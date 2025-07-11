using GharKhoj.Application.Abstracions.Authentication;

namespace GharKhoj.Api.Models.Authentication;

internal static class AuthorizationTokenMappings
{
    public static AuthorizationTokenDto ToDto(AuthorizationToken authorizationToken)
    {
        return new AuthorizationTokenDto()
        {
            AccessToken = authorizationToken.AccessToken,
            RefreshToken = authorizationToken.RefreshToken
        };
    }
}
