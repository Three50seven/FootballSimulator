using Common.Core.Domain;
using Common.Core.DTOs;
using System.Threading.Tasks;

namespace Common.Core
{
    public static class ContentRendererExtensions
    {
        /// <summary>
        /// Render content from a given path. Model/object for rendering is not required.
        /// A default, empty <see cref="object"/> is used.
        /// </summary>
        /// <param name="contentRenderer"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Task<string> RenderAsync(this IContentRenderer contentRenderer, string path)
        {
            return contentRenderer.RenderAsync(new ContentTemplate<object>(path, new object() { }));
        }

        /// <summary>
        /// Render a <see cref="Message"/> value from the given info, options, and template.
        /// Message content <see cref="Message.Content"/> is rendered from the given template and model using <see cref="IContentRenderer.RenderAsync{T}(ContentTemplate{T})"/>.
        /// </summary>
        /// <typeparam name="TTemplateModel"></typeparam>
        /// <param name="contentRenderer"></param>
        /// <param name="sendingInfo"></param>
        /// <param name="template"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static async Task<Message> RenderMessageAsync<TTemplateModel>(this IContentRenderer contentRenderer,
            MessageSendingInfo sendingInfo,
            ContentTemplate<TTemplateModel> template,
            MessageOptions options = null)
        {
            string content = await contentRenderer.RenderAsync(template);
            return new Message(sendingInfo, content, options);
        }
    }
}
