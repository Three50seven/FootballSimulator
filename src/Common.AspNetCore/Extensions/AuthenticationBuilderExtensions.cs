using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Common.Configuration;
using Common.Core.Validation;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Common.AspNetCore
{
    public static class AuthenticationBuilderExtensions
    {
        /// <summary>
        /// Add standard cookie authentication settings. 
        /// Calls <see cref="CookieExtensions.AddCookie(AuthenticationBuilder, Action{CookieAuthenticationOptions})"/> to register cookie authentication. 
        /// Reads from <see cref="AuthenticationSettings"/> for paths and expire times.
        /// Sets access denied redirects to just replace status code.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="authSettings">Custom settings to be applied to the cookie settings.</param>
        /// <param name="configureOptions">Optional callback for further customization.</param>
        /// <returns></returns>
        public static AuthenticationBuilder AddStandardCookie(
            this AuthenticationBuilder builder, 
            AuthenticationSettings authSettings = null!, 
            Action<CookieAuthenticationOptions> configureOptions = null!)
        {
            Guard.IsNotNull(builder, nameof(builder));

            authSettings ??= new AuthenticationSettings();

            return builder.AddCookie(options =>
            {
                options.AccessDeniedPath = authSettings.AccessDeniedPath;
                options.Cookie.Name = authSettings.Cookie.Name;
                options.Cookie.SecurePolicy = authSettings.Cookie.SecurePolicy;
                options.LoginPath = new PathString(authSettings.LoginPath);
                options.ExpireTimeSpan = TimeSpan.FromMinutes(authSettings.ExpirationInMinutes);
                options.Events.OnRedirectToAccessDenied = (context) =>
                {
                    // return unauthorized status code in the response instead of redirecting
                    // this is used when not authorized to access a section of the app
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return Task.FromResult(0);
                };

                configureOptions?.Invoke(options);
            });
        }

        /// <summary>
        /// Add standard cookie authentication settings. Settings are read from configuration.
        /// Calls <see cref="CookieExtensions.AddCookie(AuthenticationBuilder, Action{CookieAuthenticationOptions})"/> to register cookie authentication. 
        /// Reads from <see cref="AuthenticationSettings"/> for paths and expire times.
        /// Sets access denied redirects to just replace status code.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration">Required configuration from which the authentication settings reside.</param>
        /// <param name="sectionName">Name of section containing authentication settings. Defaults to "Authentication".</param>
        /// <param name="configureOptions">Optional callback for further customization.</param>
        /// <returns></returns>
        public static AuthenticationBuilder AddStandardCookie(
            this AuthenticationBuilder builder, 
            IConfiguration configuration,
            string sectionName = "Authentication",
            Action<CookieAuthenticationOptions> configureOptions = null!)
        {
            Guard.IsNotNull(builder, nameof(builder));
            Guard.IsNotNull(configuration, nameof(configuration));

            builder.Services.AddCustomConfigurationSettings<AuthenticationSettings>(configuration, sectionName, required: true,
                                                                                    out AuthenticationSettings authSettings);

            return AddStandardCookie(builder, authSettings, configureOptions);
        }
    }
}
