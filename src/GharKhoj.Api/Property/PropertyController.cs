using System.Dynamic;
using System.Net.Mime;
using Asp.Versioning;
using GharKhoj.Api.MimeTypes;
using GharKhoj.Api.Models.Common;
using GharKhoj.Api.Models.Properties;
using GharKhoj.Api.Services;
using GharKhoj.Api.Utils;
using GharKhoj.Application.Common;
using GharKhoj.Application.Properties.CreateProperty;
using GharKhoj.Application.Properties.GetProperties;
using GharKhoj.Application.Properties.GetProperty;
using GharKhoj.Domain.Abstractions;
using GharKhoj.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GharKhoj.Api.Property;

/// <summary>
/// Provides property related endpoints for managing property resources.
/// </summary>
[ApiController]
[ApiVersion(1)]
[Route("properties")]
[Produces(
    MediaTypeNames.Application.Json,
    MediaTypeNames.Application.Xml,
    CustomMimeTypeNames.Application.JsonV1,
    CustomMimeTypeNames.Application.HateoasJson,
    CustomMimeTypeNames.Application.HateoasJsonV1)]
public class PropertyController : ControllerBase
{
    private readonly ISender _sender;
    private readonly DataShapingService _dataShapingService;
    private readonly LinkService _linkService;

    public PropertyController(ISender sender, DataShapingService dataShapingService, LinkService linkService)
    {
        _sender = sender;
        _dataShapingService = dataShapingService;
        _linkService = linkService;
    }


    /// <summary>
    /// Retrieves a paginated list of properties based on the provided query parameters.
    /// </summary>
    /// <param name="propertiesQueryParameters">The query parameters for filtering, sorting, and pagination.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    [HttpGet]
    [ProducesResponseType<PaginationResultDto<PropertyDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HasPermission(Permissions.PropertiesRead)]
    public async Task<IActionResult> GetProperties([FromQuery] PropertiesQueryParameters propertiesQueryParameters, CancellationToken cancellationToken = default)
    {
        if (! _dataShapingService.Validate<Domain.Properties.Property>(propertiesQueryParameters.Fields))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                detail: $"Invalid fields parameter: {propertiesQueryParameters.Fields}");
        }

        var query = new GetPropertiesQuery(
                propertiesQueryParameters.Search,
                propertiesQueryParameters.Sort,
                propertiesQueryParameters.Fields, 
                propertiesQueryParameters.Page, 
                propertiesQueryParameters.PageSize);

        Result<PaginationResult<Domain.Properties.Property>> result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    detail: result.Error.Description);
        }

        var paginatedResult = PropertyMappings.ToPaginationResultDto(result.Value);

        var dataShapedPaginatedResult = new PaginationResultDto<ExpandoObject>
        {
            Items = _dataShapingService.ShapeCollectionData(
                        paginatedResult.Items,
                        propertiesQueryParameters.Fields,
                        propertiesQueryParameters.IncludeLinks
                            ? p => CreateLinksForProperty(p.Id, propertiesQueryParameters.Fields)
                            : null),
            Page = paginatedResult.Page,
            PageSize = paginatedResult.PageSize,
            TotalCount = paginatedResult.TotalCount
        };

        if (propertiesQueryParameters.IncludeLinks)
        {
            dataShapedPaginatedResult.Links = CreateLinksForProperties(
                propertiesQueryParameters, 
                dataShapedPaginatedResult.HasNextPage, 
                dataShapedPaginatedResult.HasPreviousPage);
        }

        return Ok(dataShapedPaginatedResult);
    }

    /// <summary>
    /// Retrieves a specific property by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the property to retrieve.</param>
    /// <param name="propertyQueryParameters">Query parameters for data shaping and including links.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    [HttpGet("{id}")]
    [ProducesResponseType<PropertyDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HasPermission(Permissions.PropertiesReadSingle)]
    public async Task<IActionResult> GetProperty(string id, [FromQuery] PropertyQueryParameters propertyQueryParameters, CancellationToken cancellationToken = default)
    {
        if (!_dataShapingService.Validate<Domain.Properties.Property>(propertyQueryParameters.Fields))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                detail: $"Invalid fields parameter: {propertyQueryParameters.Fields}");
        }

        var query = new GetPropertyQuery(id, propertyQueryParameters.Fields);

        Result<Domain.Properties.Property> result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.Code switch
            {
                ErrorCodes.Properties.NotFound => Problem(
                        statusCode: StatusCodes.Status404NotFound,
                        detail: result.Error.Description),
                _ => Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        detail: result.Error.Description)
            };
        }

        PropertyDto propertyDto = PropertyMappings.ToDto(result.Value);

        ExpandoObject dataShapedPropertyDto = _dataShapingService.ShapeData(propertyDto, propertyQueryParameters.Fields);

        if (propertyQueryParameters.IncludeLinks)
        {
            ((IDictionary<string, object?>)dataShapedPropertyDto)[nameof(ILinksResponseDto.Links)] =
                CreateLinksForProperty(id, query.Fields);
        }

        return Ok(dataShapedPropertyDto);
    }

    /// <summary>
    /// Creates a new property resource.
    /// </summary>
    /// <param name="createPropertyDto">The data transfer object containing the details of the property to create.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    [HttpPost]
    [ProducesResponseType<string>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HasPermission(Permissions.PropertiesCreate)]
    public async Task<ActionResult<string>> CreateProperty([FromBody] CreatePropertyDto createPropertyDto, CancellationToken cancellationToken = default)
    {
        var command = new CreatePropertyCommand(
            createPropertyDto.Tittle, 
            createPropertyDto.Description, 
            createPropertyDto.Location.Country,
            createPropertyDto.Location.State, 
            createPropertyDto.Location.City, 
            createPropertyDto.Location.Street,
            createPropertyDto.Type, 
            createPropertyDto.Price.Currency, 
            createPropertyDto.Price.Amount);

        Result<string> result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetProperty), new { id = result.Value }, result.Value)
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                detail: result.Error.Description);
    }

    private List<LinkDto> CreateLinksForProperty(string id, string? fields)
    {
        List<LinkDto> links =
        [
            _linkService.Create(nameof(GetProperty), "self", HttpMethods.Get, new { id, fields })
        ];

        return links;
    }

    private List<LinkDto> CreateLinksForProperties(PropertiesQueryParameters propertiesQueryParameters, bool hasNextPage, bool hasPreviousPage)
    {
        List<LinkDto> links =
        [
            _linkService.Create(nameof(GetProperties), "self", HttpMethods.Get, new
            {
                q = propertiesQueryParameters.Search,
                sort = propertiesQueryParameters.Sort,
                fields = propertiesQueryParameters.Fields,
                page = propertiesQueryParameters.Page,
                pageSize = propertiesQueryParameters.PageSize,
            })
        ];

        if (hasNextPage)
        {
            links.Add(_linkService.Create(nameof(GetProperties), "next-page", HttpMethods.Get, new
            {
                q = propertiesQueryParameters.Search,
                sort = propertiesQueryParameters.Sort,
                fields = propertiesQueryParameters.Fields,
                page = propertiesQueryParameters.Page + 1,
                pageSize = propertiesQueryParameters.PageSize,
            }));
        }

        if (hasPreviousPage)
        {
            links.Add(_linkService.Create(nameof(GetProperties), "previous-page", HttpMethods.Get, new
            {
                q = propertiesQueryParameters.Search,
                sort = propertiesQueryParameters.Sort,
                fields = propertiesQueryParameters.Fields,
                page = propertiesQueryParameters.Page - 1,
                pageSize = propertiesQueryParameters.PageSize,
            }));
        }

        return links;
    }
}
