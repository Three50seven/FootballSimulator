using System.Collections.Generic;

namespace Common.AspNetCore
{
    /// <summary>
    /// Resolve a list of files or bundle outputs based on a list of paths or bundles.
    /// </summary>
    public interface IBundledFileResolver
    {
        IEnumerable<string> Resolve(params string[] pathsOrBundles);
    }
}
