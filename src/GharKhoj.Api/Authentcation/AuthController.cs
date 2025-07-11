using System.Net.Mime;
using Asp.Versioning;
using GharKhoj.Api.MimeTypes;
using GharKhoj.Api.Models.Authentication;
using GharKhoj.Api.Utils;
using GharKhoj.Application.Abstracions.Authentication;
using GharKhoj.Application.Users.LoginUser;
using GharKhoj.Application.Users.RegisterUser;
using GharKhoj.Application.Users.RenewAuthorization;
using GharKhoj.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GharKhoj.Api.Authentcation;



/// <summary>
/// Provides authentication-related endpoints for user login, registration, and token refresh operations.
/// </summary>
[ApiController]
[ApiVersion(1)]
[Route("auth")]
[Produces(
    MediaTypeNames.Application.Json,
    MediaTypeNames.Application.Xml,
    CustomMimeTypeNames.Application.JsonV1)]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Authenticates a user and generates an authorization token.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType<AuthorizationTokenDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthorizationTokenDto>> LogIn([FromBody] LoginUserDto request, CancellationToken cancellationToken)
    {
        var command = new LogInUserCommand(request.Email, request.Password);

        Result<AuthorizationToken> result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(AuthorizationTokenMappings.ToDto(result.Value)) :
            result.Error.Code switch
            {
                ErrorCodes.Users.InvalidCredentials => Problem(
                    statusCode: StatusCodes.Status401Unauthorized, 
                    detail: result.Error.Description),
                _ => Problem(
                    statusCode: StatusCodes.Status400BadRequest, 
                    detail: result.Error.Description)
            };
    }

    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto request, CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password,
            request.Role);

        Result<string> result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.Error.Code switch
            {
                ErrorCodes.Users.EmailNotUnique => Problem(
                    statusCode: StatusCodes.Status409Conflict,
                    detail: result.Error.Description),
                _ => Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    detail: result.Error.Description)
            };
    }

    /// <summary>
    /// Refreshes the authorization token using the provided refresh token.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("refresh")]
    [ProducesResponseType<AuthorizationTokenDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthorizationTokenDto>> Refresh([FromBody] RefreshTokenDto request, CancellationToken cancellationToken)
    {
        var command = new RenewAuthorizationCommand(request.RefreshToken);

        Result<AuthorizationToken> result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(AuthorizationTokenMappings.ToDto(result.Value)) :
            Problem(
                statusCode: StatusCodes.Status400BadRequest,
                detail: result.Error.Description);
    }
}
