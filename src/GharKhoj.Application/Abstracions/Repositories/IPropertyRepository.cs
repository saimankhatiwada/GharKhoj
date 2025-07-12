using System.Reflection.Metadata;
using GharKhoj.Application.Common;
using GharKhoj.Domain.Abstractions;
using GharKhoj.Domain.Properties;

namespace GharKhoj.Application.Abstracions.Repositories;

public interface IPropertyRepository
{
    Task<Result<PaginationResult<Property>>> GetAllAsync(string? search, string? sort, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<Property?> GetByIdAsync(PropertyId id, CancellationToken cancellationToken = default);
    void Add(Property blog);
    void Update(Property blog);
    void Delete(Property blog);
}
