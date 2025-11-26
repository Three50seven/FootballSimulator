using Common.Core.Domain;
using System.Threading.Tasks;

namespace Common.Core
{
    /// <summary>
    /// Render string content from a given template model and path.
    /// </summary>
    public interface IContentRenderer
    {
        /// <summary>
        /// Render string content from a given template model and path.
        /// Template paths usually reference a templating file like Razor cshtml.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="template"></param>
        /// <returns></returns>
        Task<string> RenderAsync<T>(ContentTemplate<T> template);
    }
}
