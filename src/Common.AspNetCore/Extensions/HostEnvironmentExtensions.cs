using Microsoft.Extensions.Hosting;

namespace Common.AspNetCore
{
    public static class HostEnvironmentExtensions
    {
        public static bool IsTest(this IHostEnvironment environment)
        {
            return environment?.IsEnvironment("Test") ?? false;
        }
    }
}
