using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;
using System.Linq;

namespace Common.AspNetCore.Mvc
{
    /// <summary>
    /// Expand view location options by adding "VueTemplates" folder.
    /// </summary>
    public class VueTemplatesViewLocationExpander : IViewLocationExpander
    {
        private static readonly string[] _templateLocations = new string[]
        {
            "/Views/{1}/VueTemplates/{0}.cshtml",
            "/Views/Shared/VueTemplates/{0}.cshtml"
        };

        public virtual IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            return viewLocations.Union(_templateLocations);
        }

        public virtual void PopulateValues(ViewLocationExpanderContext context)
        {
            context.Values["vuetemplatesviewlocation"] = nameof(VueTemplatesViewLocationExpander);
        }
    }
}
