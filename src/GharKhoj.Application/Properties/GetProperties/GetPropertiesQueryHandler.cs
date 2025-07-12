using GharKhoj.Application.Abstracions.Messaging;
using GharKhoj.Application.Abstracions.Repositories;
using GharKhoj.Application.Common;
using GharKhoj.Domain.Abstractions;
using GharKhoj.Domain.Properties;

namespace GharKhoj.Application.Properties.GetProperties;

internal sealed class GetPropertiesQueryHandler : IQueryHandler<GetPropertiesQuery, PaginationResult<Property>>
{
    private readonly IPropertyRepository _propertyRepository;
    public GetPropertiesQueryHandler(IPropertyRepository propertyRepository)
    {
        _propertyRepository = propertyRepository;
    }

    public async Task<Result<PaginationResult<Property>>> Handle(GetPropertiesQuery request, CancellationToken cancellationToken)
    {
        Result<PaginationResult<Property>> result = await _propertyRepository
            .GetAllAsync(request.Search, request.Sort, request.Page ?? 1, request.PageSize ?? 10, cancellationToken);

        return result.IsSuccess 
            ? result.Value
            : Result.Failure<PaginationResult<Property>>(result.Error);
    }
}
