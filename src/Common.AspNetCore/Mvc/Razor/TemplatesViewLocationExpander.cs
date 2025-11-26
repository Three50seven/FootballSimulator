using Microsoft.AspNetCore.Mvc.Razor;

namespace Common.AspNetCore.Mvc
{
    /// <summary>
    /// Expand view location options by adding "DisplayTemplates" and "EditorTemplates" folder.
    /// </summary>
    public class TemplatesViewLocationExpander : IViewLocationExpander
    {
        private static readonly string[] _templateLocations = new string[]
        {
            "/Views/{1}/DisplayTemplates/{0}.cshtml",
            "/Views/{1}/EditorTemplates/{0}.cshtml",
            "/Views/Shared/DisplayTemplates/{0}.cshtml",
            "/Views/Shared/EditorTemplates/{0}.cshtml"
        };

        public virtual IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            return viewLocations.Union(_templateLocations);
        }

        public virtual void PopulateValues(ViewLocationExpanderContext context)
        {
            context.Values["templatesviewlocation"] = nameof(TemplatesViewLocationExpander);
        }
    }
}
