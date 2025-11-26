using System.Collections.Generic;

namespace Common.AspNetCore
{
    /// <summary>
    /// Default, basic implementation that simply returns the paths or bundles as their original selves.
    /// </summary>
    public class DirectBundledFileResolver : IBundledFileResolver
    {

        public IEnumerable<string> Resolve(params string[] pathsOrBundles)
        {
            return pathsOrBundles;
        }
    }
}
