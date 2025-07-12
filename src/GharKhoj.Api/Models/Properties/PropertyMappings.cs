using GharKhoj.Api.Models.Common;
using GharKhoj.Application.Common;

namespace GharKhoj.Api.Models.Properties;

internal static class PropertyMappings
{
    public static PaginationResultDto<PropertyDto> ToPaginationResultDto(PaginationResult<Domain.Properties.Property> paginationResult)
    {
        var proportiesDto = paginationResult
            .Items
            .Select(property => new PropertyDto
            {
                Id = property.Id.Value,
                Tittle = property.Tittle.Value,
                Description = property.Description.Value,
                Location = new LocationDto(
                    property.Location.Country, 
                    property.Location.State, 
                    property.Location.City, 
                    property.Location.Street),
                Type = (int)property.Type,
                Price = new MoneyDto(
                    property.Price.Amount, 
                    property.Price.Currency.Code)
            })
            .ToList();

        return PaginationResultDto<PropertyDto>.Create(
            proportiesDto, 
            paginationResult.Page, 
            paginationResult.PageSize, 
            paginationResult.TotalCount);
    }

    public static PropertyDto ToDto(Domain.Properties.Property property)
    {
        return new PropertyDto
        {
            Id = property.Id.Value,
            Tittle = property.Tittle.Value,
            Description = property.Description.Value,
            Location = new LocationDto(
                property.Location.Country, 
                property.Location.State, 
                property.Location.City, 
                property.Location.Street),
            Type = (int)property.Type,
            Price = new MoneyDto(
                property.Price.Amount, 
                property.Price.Currency.Code)
        };
    }
}
