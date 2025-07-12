using System.Dynamic;
using System.Net.Mime;
using Asp.Versioning;
using Azure;
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

    [HttpGet("{id}")]
    [ProducesResponseType<PropertyDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HasPermission(Permissions.PropertiesReadSingle)]
    public async Task<IActionResult> GetProperty(string id, [FromQuery] PropertyQueryParameters propertyQueryParameters, CancellationToken cancellationToken = default)
    {
        var query = new GetPropertyQuery(id, propertyQueryParameters.Fields);

        Result<Domain.Properties.Property> result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Ok(PropertyMappings.ToDto(result.Value))
            : result.Error.Code switch
            {
                ErrorCodes.Properties.NotFound => Problem(
                        statusCode: StatusCodes.Status404NotFound,
                        detail: result.Error.Description),
                _ => Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        detail: result.Error.Description)
            };
    }

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
