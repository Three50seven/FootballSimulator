using Common.Core.Domain;
using Common.Core.Interfaces;
using Common.Core.Validation;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Core.Services
{
    /// <summary>
    /// Message queuing service that will save a Message to a repository and then queue the sending 
    /// task for that message in a Task Queue <see cref="ITaskQueue"/> under service <see cref="MessageBackgroundTaskSender"/>.
    /// </summary>
    public class MessageTaskQueuingService : MessageServiceBase
    {
        public MessageTaskQueuingService(
            IContentRenderer renderer, 
            MessageValidatingInfo validatingInfo, 
            ICommandRepository<Message> messageRepository,
            ITaskQueue taskQueue) 
            : base(renderer, validatingInfo)
        {
            MessageRepository = messageRepository;
            TaskQueue = taskQueue;
        }

        protected ICommandRepository<Message> MessageRepository { get; }
        protected ITaskQueue TaskQueue { get; }

        public override async Task<Message> ProcessAsync(Message message, MessageValidatingInfo validatingInfo = null)
        {
            Guard.IsNotNull(message, nameof(message));

            Validate(message, validatingInfo);

            await MessageRepository.AddOrUpdateAsync(message);

            TaskQueue.Enqueue<MessageBackgroundTaskSender>(sender => sender.SendAsync(message.Guid, CancellationToken.None));

            return message;
        }
    }
}
