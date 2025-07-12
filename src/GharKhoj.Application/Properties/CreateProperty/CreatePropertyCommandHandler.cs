using GharKhoj.Application.Abstracions.Authentication;
using GharKhoj.Application.Abstracions.Messaging;
using GharKhoj.Application.Abstracions.Repositories;
using GharKhoj.Domain.Abstractions;
using GharKhoj.Domain.Properties;
using GharKhoj.Domain.Users;

namespace GharKhoj.Application.Properties.CreateProperty;

internal sealed class CreatePropertyCommandHandler : ICommandHandler<CreatePropertyCommand, string>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePropertyCommandHandler(
        IPropertyRepository propertyRepository, 
        IUserRepository userRepository, 
        IUserContext userContext, 
        IUnitOfWork unitOfWork)
    {
        _propertyRepository = propertyRepository;
        _userRepository = userRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<string>> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetByIdAsync(new UserId(_userContext.UserId), cancellationToken);

        if (user is null)
        {
            return Result.Failure<string>(PropertyErrors.UserNotExists(_userContext.UserId));
        }

        var property = Property.Create(
            user.Id,
            new Tittle(request.Tittle),
            new Description(request.Desciption),
            new Location(request.Country, request.State, request.City, request.Street),
            (PropertyType)request.Type,
            new Money(request.Amount, Currency.FromCode(request.Currency)));

        _propertyRepository.Add(property);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return property.Id.Value;
    }
}
