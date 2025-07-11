using GharKhoj.Application.Abstracions.Authentication;
using GharKhoj.Application.Abstracions.Messaging;
using GharKhoj.Application.Abstracions.Repositories;
using GharKhoj.Domain.Abstractions;
using GharKhoj.Domain.Users;

namespace GharKhoj.Application.Users.RegisterUser;

internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, string>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(
        IAuthenticationService authenticationService, 
        IUserRepository userRepository, 
        IUnitOfWork unitOfWork)
    {
        _authenticationService = authenticationService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = User.RegisterUser(
            new FullName(request.FullName),
            new Email(request.Email),
            Role.FormRole(request.Role));

        Result<string> identityId = await _authenticationService.RegisterAsync(
            user,
            request.Password,
            cancellationToken);

        if (identityId.IsFailure)
        {
            return Result.Failure<string>(identityId.Error);
        }

        user.SetIdentityId(identityId.Value);

        _userRepository.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id.Value;
    }
}
