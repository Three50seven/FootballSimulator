using System;
using System.Collections.Generic;
using System.Text;

namespace Common.AspNetCore
{
    public class AuthenticationSettings
    {
        public string AccessDeniedPath { get; set; } = "/access-denied";
        public string LoginPath { get; set; } = "/auth/login/";
        public int ExpirationInMinutes { get; set; } = 60;
        public AuthenticationCookieSettings Cookie { get; set; } = new AuthenticationCookieSettings();
    }
}
