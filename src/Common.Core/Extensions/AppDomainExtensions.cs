using Common.Core.Validation;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Common.Core
{
    public static class AppDomainExtensions
    {
        /// <summary>
        /// Experimental extension method that will get the full path of the given solution folder root director.
        /// </summary>
        /// <param name="domain">Current app domain.</param>
        /// <param name="levelsDeep">How many levels deep the calling method's file exists.</param>
        /// <param name="levelsDeepIfInBin">How many levels deep if path is found in a bin folder.</param>
        /// <returns></returns>
        public static string GetProjectSolutionRoot(this AppDomain domain, int levelsDeep = 1, int levelsDeepIfInBin = 2)
        {
            if (domain == null)
                return null;

            var sbLevels = new StringBuilder();

            for (int i = 0; i < levelsDeep; i++)
            {
                sbLevels.Append("..\\");
            }

            var dir = Path.GetFullPath(Path.Combine(domain.BaseDi‌​rectory, sbLevels.ToString()));

            if (levelsDeepIfInBin > 0 && (dir.EndsWith("bin") || dir.EndsWith(@"bin\")))
            {
                var sbBinLevels = new StringBuilder();
                for (int i = 0; i < levelsDeepIfInBin; i++)
                {
                    sbBinLevels.Append("..\\");
                }
                dir = Path.GetFullPath(Path.Combine(dir, sbBinLevels.ToString()));
            }

            return dir;
        }

        /// <summary>
        /// Load in all referenced assemblies in the AppDomain based on provided query to filter only specific assembly set, 
        /// typically the application assemblies.
        /// Ref: https://dotnetstories.com/blog/Dynamically-pre-load-assemblies-in-a-ASPNET-Core-or-any-C-project-en-7155735300
        /// </summary>
        /// <param name="appDomain">Valid appdomain, typically the current AppDomain.</param>
        /// <param name="assemblyNamesQuery">Required query on the assembly name. Return true if the assembly under the provided assembly name should be loaded.</param>
        /// <param name="includeFramework">Whether to include .NET Framework, Microsoft, System assemblies, etc. Defaults to false.</param>
        public static void LoadAllAssemblies(
            this AppDomain appDomain,
            Func<string, bool> assemblyNamesQuery,
            bool includeFramework = false)
        {
            const string _logPrefix = " - Assembly Loading: ";

            Guard.IsNotNull(appDomain, nameof(appDomain));
            Guard.IsNotNull(assemblyNamesQuery, nameof(assemblyNamesQuery));

            // Storage to ensure not loading the same assembly twice and optimize calls to GetAssemblies()
            var loaded = new ConcurrentDictionary<string, bool>();

            // Filter to avoid loading all the .net framework
            bool ShouldLoad(string assemblyName)
            {
                return (includeFramework || NotNetFramework(assemblyName))
                    && !loaded.ContainsKey(assemblyName)
                    && assemblyNamesQuery.Invoke(assemblyName);
            }

            bool NotNetFramework(string assemblyName)
            {
                return !assemblyName.StartsWith("Microsoft.")
                    && !assemblyName.StartsWith("System.")
                    && !assemblyName.StartsWith("Newtonsoft.")
                    && assemblyName != "netstandard";
            }

            void LoadReferencedAssembly(Assembly assembly)
            {
                // Check all referenced assemblies of the specified assembly
                foreach (AssemblyName an in assembly.GetReferencedAssemblies().Where(a => ShouldLoad(a.FullName)))
                {
                    // Load the assembly and load its dependencies
                    LoadReferencedAssembly(Assembly.Load(an));
                    loaded.TryAdd(an.FullName, true);
                    Debug.WriteLine($"{_logPrefix} > Referenced assembly => {an.FullName}");
                }
            }

            // Populate already loaded assemblies
            Debug.WriteLine($"{_logPrefix} Already loaded assemblies:");
            foreach (var a in appDomain.GetAssemblies().Where(a => ShouldLoad(a.FullName)))
            {
                loaded.TryAdd(a.FullName, true);
                Debug.WriteLine($"{_logPrefix} > {a.FullName}");
            }

            int alreadyLoaded = loaded.Keys.Count();
            Stopwatch sw = new Stopwatch();

            // Loop on loaded assemblies to load dependencies (it includes Startup assembly so should load all the dependency tree) 
            foreach (Assembly assembly in appDomain.GetAssemblies().Where(a => NotNetFramework(a.FullName)))
            {
                LoadReferencedAssembly(assembly);
            }

            // Debug
            Debug.WriteLine($"{_logPrefix} Assemblies loaded after scan ({(loaded.Keys.Count - alreadyLoaded)} assemblies in {sw.ElapsedMilliseconds} ms):");
            foreach (var a in loaded.Keys.OrderBy(k => k))
            {
                Debug.WriteLine($"{_logPrefix} > {a}");
            }
        }
    }
}
