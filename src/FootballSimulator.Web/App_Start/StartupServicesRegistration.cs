using Common.AspNetCore;
using Common.Configuration;
using Common.Core;
using Common.Cache;
using Common.Core.Validation;
using Common.Serialization.SystemTextJson;
using FootballSimulator.Application.User;
using FootballSimulator.Core.Interfaces;
using FootballSimulator.Infrastructure.Data;
using FootballSimulator.Web.Components.Account;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Text.Json;
using Common.AspNetCore.Mvc;
using FootballSimulator.Core.Services;
using FootballSimulator.Application.Services;
using FootballSimulator.Web.Providers;

namespace FootballSimulator.Web
{
    internal static class StartupServicesRegistration
    {
        public static IServiceCollection Register(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            // Add services to the container.
            AppDomain.CurrentDomain.LoadAllAssemblies((assemblyName) => assemblyName.StartsWith("FootballSimulator"));

            AddCommonCoreServices(services, configuration);
            AddAspNetCoreServices(services, configuration, hostEnvironment);
            AddAuthentication(services, configuration);
            AddThirdPartyServices(services, configuration);
            AddAppServices(services, configuration);

            return services;
        }

        private static IServiceCollection AddCommonCoreServices(IServiceCollection services, IConfiguration configuration)
        {
            // Registers "appsettings" (standard configuration key-value pairs) to ISettings.
            // Appsettings are configured from appsettings.json section "appsettings".
            // Also, custom class is provided to these key value pairs to allow for more defined list of generic settings
            services.AddAppSettings<FootballSimulationAppSettings>();

            // Common.Core common services registration
            services.AddCommonCoreServices()
                .AddCoreServicesWithDependencies();

            // Adds implementation for ICache
            // Profiles set up under DataCache in appsettings.json
            services.AddDataCache(configuration);

            // Registers clientside/javascript settings, pulled from "ClientSideSettings" in appsettings.json 
            services.AddClientSideSettings(configuration);

            return services;
        }

        private static IServiceCollection AddAspNetCoreServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            JsonSerializerOptions DefaultJsonOptions;
            DefaultJsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = null
            };
            DefaultJsonOptions.Converters.Add(new DateTimeConverter());

            // Register ISerializer under Microsoft's System.Text.Json serializer
            // Will scan all custom assemblies for types that should allow for custom attribute serialization
            // Use Common.SerializeAttribute on classes that need custom serialization
            services.AddSystemTextSerializer(
                        options: DefaultJsonOptions);

            services.AddRouting(options =>
            {
                options.AppendTrailingSlash = true;
                options.LowercaseUrls = true;
            });


            //TODO: These came from Blazor template, verify if needed
            // Add services to the container.
            services.AddRazorComponents()
                .AddInteractiveServerComponents();

            services.AddCascadingAuthenticationState();
            services.AddScoped<IdentityUserAccessor>();
            services.AddScoped<IdentityRedirectManager>();
            services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();



            services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<FootballSimulatorDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
            // TODO: End Blazor template code



            // Configure Razor Engine
            // Adding custom templates location finder for "DisplayTemplates" and "EditorTemplates" design
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new TemplatesViewLocationExpander());
                options.ViewLocationExpanders.Add(new VueTemplatesViewLocationExpander());
            });


            var mvcBuilder = services.AddControllersWithViews((options) =>
            {
                // handle binding DateTimes to models, adjusting to UTC
                options.ModelBinderProviders.Insert(0, new DateTimeUTCBinderProvider());
            })
            .AddJsonOptions((options) =>
            {
                // NOTE: Static default settings at the top of this file also 
                //       need to be updated when any changes are made here and vice versa.

                options.JsonSerializerOptions.PropertyNameCaseInsensitive = DefaultJsonOptions.PropertyNameCaseInsensitive;
                options.JsonSerializerOptions.PropertyNamingPolicy = DefaultJsonOptions.PropertyNamingPolicy;
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
            });

#if DEBUG
            // allow for editing razor pages without requiring a recompile
            //mvcBuilder.AddRazorRuntimeCompilation();
#endif

            // add custom data protection settings for non-dev environments
            // this is required so the antiforgery keys are properly configured for the IIS server environments
            if (!environment.IsLocal())
                services.AddDataProtection(configuration);

            return services;
        }

        private static IServiceCollection AddAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            Guard.IsNotNull(services, nameof(services));

            // Authentication settings
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();


            services.AddAuthentication(AuthConstants.DefaultScheme)
                    .AddStandardCookie(configuration, configureOptions: options =>
                    {
                        var expirationTime = configuration.GetValue<int>("Authentication:TimeoutMinutes");

                        options.ExpireTimeSpan = TimeSpan.FromMinutes(expirationTime);
                        options.SlidingExpiration = false;
                        options.Cookie.MaxAge = TimeSpan.FromMinutes(expirationTime);
                    });

            services.AddScoped<IUserClaimsService, UserClaimsService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IIdentitySignOnService, IdentitySignOnService>();

#pragma warning disable CS8603 // Possible null reference argument.
            services.AddCurrentUser<IApplicationUser>(serviceProvider => serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext?.ApplicationUser());
#pragma warning restore CS8603 // Possible null reference argument.

            return services;
        }

        private static IServiceCollection AddThirdPartyServices(IServiceCollection services, IConfiguration configuration)
        {
            //            // Register front-end asset rendering services
            //            services.AddBunder(configuration, sectionName: "Bundling");
            //            services.AddSingleton<IBundledFileResolver, BunderBundledFileResolver>();

//#if MINI_PROFILER_ENABLED
//                        services.AddMiniProfiler().AddEntityFramework();
//                        if (MiniProfiler.Current != null)
//                        {
//                            services.AddScoped<IProfiler>((serviceProvider)
//                                => new MiniProfilerProfiler(MiniProfiler.Current));
//                        }
//#else
//            services.AddScoped<IProfiler, EmptyProfiler>();
//#endif

            return services;
        }

        private static IServiceCollection AddAppServices(IServiceCollection services, IConfiguration configuration)
        {
            Guard.IsNotNull(services, nameof(services));

            // add all custom App services here
            services.AddScoped<IUserLoginRecorder, UserLoginRecorder>();
            services.AddScoped<IThemeService, ThemeService>();
            //etc.

            // database and repositories
            services.AddEFDataAccess(configuration)
                .AddSqlClientDataAccess(configuration);

            var applicationServiceTypes = typeof(ApplicationUserPrincipal).Assembly.GetTypeMatches(true, t => t != null
                                                                                                            && t.FullName != null
                                                                                                            && t.Namespace != null
                                                                                                            && (t.FullName.EndsWith("Service")
                                                                                                           || t.Namespace.EndsWith("Service")
                                                                                                           || t.FullName.EndsWith("Services")
                                                                                                           || t.Namespace.EndsWith("Services")
                                                                                                           || t.Namespace.EndsWith("Validator")
                                                                                                           || t.FullName.EndsWith("Validator")));
            services.AddTypeMatches(applicationServiceTypes, ServiceLifetime.Scoped);

            return services;
        }
    }
}
