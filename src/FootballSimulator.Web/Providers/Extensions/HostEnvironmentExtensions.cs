using FootballSimulator.Core;

namespace FootballSimulator.Web
{
    public static class HostEnvironmentExtensions
    {
        /// <summary>
        /// Whether environment is considered the "Local" development environment
        /// </summary>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static bool IsLocal(this IHostEnvironment environment)
        {
            return environment?.IsEnvironment(nameof(HostEnvironmentOption.Local)) ?? false;
        }

        public static bool IsTest(this IHostEnvironment environment)
        {
            return environment?.IsEnvironment(nameof(HostEnvironmentOption.Test)) ?? false;
        }
    }
}
