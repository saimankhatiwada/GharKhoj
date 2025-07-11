using GharKhoj.Domain.Abstractions;

namespace GharKhoj.Domain.Users;

public static class UserErrors
{

    public static Error NotFound(string Value) => new(
        "Users.NotFound", 
        $"The user with the Id = '{Value}' was not found");

    public static Error EmailNotUnique => new(
        "Users.EmailNotUnique",
        "The provided email is not unique");

    public static Error InvalidCredentials => new(
        "Users.InvalidCredentials",
        "The provided credentials are not correct");

    public static Error InvalidRefreshToken => new(
        "Users.InvalidRefreshToken",
        "The provided refresh-token is not valid");
}
