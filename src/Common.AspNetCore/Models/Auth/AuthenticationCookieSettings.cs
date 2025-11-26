using Microsoft.AspNetCore.Http;

namespace Common.AspNetCore
{
    public class AuthenticationCookieSettings
    {
        public string Name { get; set; } = "appauth";
        public CookieSecurePolicy SecurePolicy { get; set; } = CookieSecurePolicy.None;
    }
}
