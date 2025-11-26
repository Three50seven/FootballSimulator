using Common.Core.Domain;
using Common.Core.DTOs;
using System.Threading.Tasks;

namespace Common.Core
{
    public interface IMessageService 
    {
        /// <summary>
        /// Process a <see cref="Message"/> based on sending info, a template/model to render, options, and optional validating info.
        /// </summary>
        /// <typeparam name="TTemplateModel"></typeparam>
        /// <param name="sendingInfo">Required standard sending info (to, from, etc.).</param>
        /// <param name="template">Template + model to render the content of the message.</param>
        /// <param name="options">Optional options for customizing <see cref="Message"/> record / sending.</param>
        /// <param name="validatingInfo">Optional validating info to impose on the resulting <see cref="Message"/>.</param>
        /// <returns></returns>
        Task<Message> ProcessAsync<TTemplateModel>(
            MessageSendingInfo sendingInfo, 
            ContentTemplate<TTemplateModel> template,
            MessageOptions options = null,
            MessageValidatingInfo validatingInfo = null)
            where TTemplateModel : class;

        /// <summary>
        /// Process a <see cref="Message"/> with optional validating info to apply.
        /// </summary>
        /// <param name="message">Message object to process (save, send, queue, etc.)</param>
        /// <param name="validatingInfo">Optional validating info to impose on the resulting <see cref="Message"/>.</param>
        /// <returns></returns>
        Task<Message> ProcessAsync(Message message, MessageValidatingInfo validatingInfo = null);
    }
}
