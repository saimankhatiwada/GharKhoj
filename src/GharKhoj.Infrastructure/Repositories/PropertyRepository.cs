using GharKhoj.Application.Abstracions.Repositories;
using GharKhoj.Application.Common;
using GharKhoj.Domain.Abstractions;
using GharKhoj.Domain.Properties;
using GharKhoj.Infrastructure.Sorting;
using GharKhoj.Infrastructure.Sorting.Mappings;
using Microsoft.EntityFrameworkCore;

namespace GharKhoj.Infrastructure.Repositories;

internal sealed class PropertyRepository : Repository<Property, PropertyId>, IPropertyRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly SortMappingProvider _sortMappingProvider;
    public PropertyRepository(ApplicationDbContext dbContext, SortMappingProvider sortMappingProvider) : base(dbContext) 
    {
        _dbContext = dbContext;
        _sortMappingProvider = sortMappingProvider;
    }
    public async Task<Result<PaginationResult<Property>>> GetAllAsync(string? search, string? sort, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        search ??= search?.Trim().ToLower();

        if (!_sortMappingProvider.ValidateMappings<Property>(sort))
        {
            return Result.Failure<PaginationResult<Property>>(SortMappingErrors.MappingFailed);
        }

        SortMapping[] sortMappings = _sortMappingProvider.GetMappings<Property>();

        IQueryable<Property> propertyQuery = _dbContext
            .Properties
            .Where(p => search == null ||
                        EF.Functions.Like(p.Tittle, $"%{search}%") ||
                        EF.Functions.Like(p.Description, $"%{search}%"))
            .ApplySort(sort, sortMappings);

        int totalCount = await _dbContext.Properties.CountAsync(cancellationToken);

        List<Property> properties = await propertyQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var paginatedProperties = PaginationResult<Property>.Create(properties, page, pageSize, totalCount);

        return paginatedProperties;
    }
}
