using GharKhoj.Domain.Properties;

namespace GharKhoj.Infrastructure.Sorting.Mappings;

internal static class PropertySortMapping
{
    public static readonly SortMappingDefinition<Property> SortMapping = new()
    {
        Mappings =
        [
            new SortMapping(nameof(Property.Tittle)),
            new SortMapping(nameof(Property.Price.Amount)),
            new SortMapping(nameof(Property.Price.Currency)),
        ]
    };
}
