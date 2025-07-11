using System.Net;
using System.Net.Http.Json;
using GharKhoj.Application.Abstracions.Authentication;
using GharKhoj.Domain.Abstractions;
using GharKhoj.Domain.Users;
using GharKhoj.Infrastructure.Authentication.Models;

namespace GharKhoj.Infrastructure.Authentication;

internal sealed class AuthenticationService : IAuthenticationService
{
    private const string PasswordCredentialType = "password";
    private readonly HttpClient _httpClient;
    public AuthenticationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result> DeleteUserAsync(string identityId, CancellationToken cancellationToken = default)
    {
        try
        {
            HttpResponseMessage result = await _httpClient.DeleteAsync($"users/{identityId}", cancellationToken);

            result.EnsureSuccessStatusCode();

            return Result.Success();
        }
        catch (HttpRequestException e)
            when (e.StatusCode == HttpStatusCode.NotFound)
        {
            return Result.Failure(UserErrors.NotFound(identityId));
        }
        catch (HttpRequestException)
        {
            return Result.Failure(UserErrors.ThirdParty("Keycloak"));
        }
    }

    public async Task<Result<string>> RegisterAsync(User user, string password, CancellationToken cancellationToken = default)
    {
        var userRepresentationModel = UserRepresentationModel.FromUser(user);

        userRepresentationModel.Credentials =
        [
            new()
            {
                Value = password,
                Temporary = false,
                Type = PasswordCredentialType
            }
        ];

        try
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
                "users",
                userRepresentationModel,
                cancellationToken);

            return ExtractIdentityIdFromLocationHeader(response);
        }
        catch (HttpRequestException e)
            when (e.StatusCode == HttpStatusCode.Conflict)
        {
            return Result.Failure<string>(UserErrors.EmailNotUnique);
        }
        catch (HttpRequestException e)
            when (e.StatusCode == HttpStatusCode.BadRequest)
        {
            return Result.Failure<string>(UserErrors.ThirdParty("Keycloak"));
        }
    }

    private static string ExtractIdentityIdFromLocationHeader(HttpResponseMessage httpResponseMessage)
    {
        const string usersSegmentName = "users/";

        string? locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery
                                 ?? throw new InvalidOperationException("Location header can't be null");

        int userSegmentValueIndex = locationHeader.IndexOf(
            usersSegmentName,
            StringComparison.InvariantCultureIgnoreCase);

        string userIdentityId = locationHeader[
            (userSegmentValueIndex + usersSegmentName.Length)..];

        return userIdentityId;
    }
}
