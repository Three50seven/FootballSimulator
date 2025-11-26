namespace Common.AspNetCore
{
    public class AuthStatusHeaderOptions
    {
        public const string DefaultHeaderName = "USER_AUTH_STATUS";

        public string HeaderName { get; set; } = DefaultHeaderName;
    }
}
