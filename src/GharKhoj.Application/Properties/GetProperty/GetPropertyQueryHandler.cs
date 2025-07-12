using GharKhoj.Application.Abstracions.Messaging;
using GharKhoj.Application.Abstracions.Repositories;
using GharKhoj.Domain.Abstractions;
using GharKhoj.Domain.Properties;

namespace GharKhoj.Application.Properties.GetProperty;

internal sealed class GetPropertyQueryHandler : IQueryHandler<GetPropertyQuery, Property>
{
    private readonly IPropertyRepository _propertyRepository;

    public GetPropertyQueryHandler(IPropertyRepository propertyRepository)
    {
        _propertyRepository = propertyRepository;
    }

    public async Task<Result<Property>> Handle(GetPropertyQuery request, CancellationToken cancellationToken)
    {
        Property? result = await _propertyRepository.GetByIdAsync(new PropertyId(request.PropertyId), cancellationToken);

        return result is not null 
            ? result 
            : Result.Failure<Property>(PropertyErrors.NotFound(request.PropertyId));
    }
}
