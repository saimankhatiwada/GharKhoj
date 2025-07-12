namespace GharKhoj.Api.Utils;

public static class ErrorCodes
{
    public static class Users
    {
        public const string NotFound = "User.NotFound";
        public const string InvalidCredentials = "Users.InvalidCredentials";
        public const string EmailNotUnique = "Users.EmailNotUnique";
        public const string InvalidRefreshToken = "Users.InvalidRefreshToken";
        public const string ThirdParty = "Users.ThirdParty";
    }

    public static class Properties
    {
        public const string NotFound = "Properties.NotFound";
    }
}
