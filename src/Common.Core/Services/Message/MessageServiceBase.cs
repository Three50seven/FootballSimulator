using Common.Core.Domain;
using Common.Core.DTOs;
using Common.Core.Validation;
using System.Threading.Tasks;

namespace Common.Core.Services
{
    /// <summary>
    /// Abstract service base that holds core logic around processing a message to be saved, queued, sent, etc.
    /// Provides default "validating" logic using <see cref="MessageValidatingInfo"/> to process messages 
    /// under a "test mode" environment versus a production environment.
    /// </summary>
    public abstract class MessageServiceBase : IMessageService
    {
        protected MessageServiceBase(
            IContentRenderer renderer, 
            MessageValidatingInfo validatingInfo)
        {
            Renderer = renderer;
            ValidatingInfo = validatingInfo;
        }

        protected IContentRenderer Renderer { get; }
        protected MessageValidatingInfo ValidatingInfo { get; }

        /// <summary>
        /// Call into <see cref="Message.Validate(MessageValidatingInfo)"/> using the optional <paramref name="validatingInfo"/>
        /// or the default <see cref="ValidatingInfo"/> property on the class/application level.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="validatingInfo"></param>
        /// <returns></returns>
        protected virtual Message Validate(Message message, MessageValidatingInfo validatingInfo = null)
        {
            message.Validate(validatingInfo ?? ValidatingInfo);
            return message;
        }

        public virtual async Task<Message> ProcessAsync<TTemplateModel>(
            MessageSendingInfo sendingInfo,
            ContentTemplate<TTemplateModel> template,
            MessageOptions options = null,
            MessageValidatingInfo validatingInfo = null)
            where TTemplateModel : class
        {
            var message = await Renderer.RenderMessageAsync(sendingInfo, template, options);
            return await ProcessAsync(message, validatingInfo);
        }

        public virtual Task<Message> ProcessAsync(
            Message message, 
            MessageValidatingInfo validatingInfo = null)
        {
            Guard.IsNotNull(message, nameof(message));

            Validate(message, validatingInfo);

            return Task.FromResult(message);
        }
    }
}
