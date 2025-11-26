using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace Common.AspNetCore.Mvc
{
    public static class ViewEngineExtensions
    {
        /// <summary>
        /// Checks if View file specified by <paramref name="viewPath"/> exists.
        /// </summary>
        /// <param name="viewEngine"></param>
        /// <param name="viewPath"></param>
        /// <returns></returns>
        public static bool ViewExists(this IViewEngine viewEngine, string viewPath)
        {
            if (viewEngine == null || string.IsNullOrWhiteSpace(viewPath))
                return false;

            return viewEngine.GetView("", viewPath, false)?.Success ?? false;
        }
    }
}
