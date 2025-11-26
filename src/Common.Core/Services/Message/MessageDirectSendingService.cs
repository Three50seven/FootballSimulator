using Common.Core.Domain;
using System;
using System.Threading.Tasks;

namespace Common.Core.Services
{
    /// <summary>
    /// Message service that saves <see cref="Message"/> record to repository
    /// and sends the message immediately after using <see cref="IMessageSender"/>.
    /// Message failed/success state is updated after send attempt.
    /// This service is used when <see cref="MessageSettings.QueueType"/> is <see cref="MessageQueueOption.DirectSend"/>.
    /// </summary>
    public class MessageDirectSendingService : MessageServiceBase 
    {
        public MessageDirectSendingService(
            ICommandRepository<Message> messageRepository, 
            IContentRenderer contentRenderer, 
            MessageValidatingInfo validatingInfo,
            IMessageSender messageSender)
            : base(contentRenderer, validatingInfo)
        {
            MessageRepository = messageRepository;
            MessageSender = messageSender;
        }

        protected ICommandRepository<Message> MessageRepository { get; }
        protected IMessageSender MessageSender { get; }

        public override async Task<Message> ProcessAsync(Message message, MessageValidatingInfo validatingInfo = null)
        {
            if (message == null)
                return null;

            Validate(message, validatingInfo);

            await MessageRepository.AddOrUpdateAsync(message);

            var result = await MessageSender.SendAsync(message);
            if (result == null)
                throw new NullReferenceException(nameof(MessageSendResult));

            if (!result.Succeeded)
                message.Fail(result.Exception?.GetFriendlyMessage());
            else
                message.Complete();

            return message;
        }
    }
}
