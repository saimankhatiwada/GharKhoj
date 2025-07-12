namespace GharKhoj.Infrastructure.Sorting;

#pragma warning disable S2326 // Unused type parameters should be removed
public sealed class SortMappingDefinition<TSource> : ISortMappingDefinition
#pragma warning restore S2326 // Unused type parameters should be removed
{
    public required SortMapping[] Mappings { get; init; }
}
