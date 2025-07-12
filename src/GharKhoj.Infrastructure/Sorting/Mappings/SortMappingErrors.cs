using GharKhoj.Domain.Abstractions;

namespace GharKhoj.Infrastructure.Sorting.Mappings;

public static class SortMappingErrors
{
    public static readonly Error MappingFailed = new(
        "Sort.MappingFailed",
        "The provided sort parameter isn't valid.");
}
