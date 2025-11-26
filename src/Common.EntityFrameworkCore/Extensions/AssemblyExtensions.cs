using Common.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.EntityFrameworkCore
{
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Return list of type matches that contain all applicable repository interfaces, include generics and the associated repository implementation.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="query">Query to filter repository types. Defaults to ending with "Repository".</param>
        /// <returns></returns>
        public static IEnumerable<TypeRegistrationMatch> GetRepositoryTypeMatches(this Assembly assembly, Func<Type, bool> query = null)
        {
            if (assembly == null)
                return [];

            query ??= (t) => t.FullName.EndsWith("Repository");

            var matches = new List<TypeRegistrationMatch>();

            var types = from type in assembly.GetExportedTypes()
                        where type.IsClass && !type.IsAbstract && query.Invoke(type)
                        select new
                        {
                            Type = type,
                            Interfaces = type.GetInterfaces()
                        };

            foreach (var type in types)
            {
                foreach (var @interface in type.Interfaces)
                {
                    matches.Add(new TypeRegistrationMatch(@interface, type.Type));
                }
            }

            return matches;
        }
    }
}
