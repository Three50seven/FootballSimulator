using Common.Core.Domain;
using Common.Core.Validation;
using System.Threading.Tasks;

namespace Common.Core.Services
{
    /// <summary>
    /// Message service that simply saves <see cref="Message"/> record to database repository.
    /// No sending or other actions.
    /// This service is used when <see cref="MessageSettings.QueueType"/> is <see cref="MessageQueueOption.DatabaseOnly"/>.
    /// </summary>
    public class MessageDatabaseOnlyService : MessageServiceBase
    {
        public MessageDatabaseOnlyService(
            IContentRenderer renderer, 
            MessageValidatingInfo validatingInfo,
            ICommandRepository<Message> messageRepository) 
            : base(renderer, validatingInfo)
        {
            MessageRepository = messageRepository;
        }

        protected ICommandRepository<Message> MessageRepository { get; }

        public override async Task<Message> ProcessAsync(Message message, MessageValidatingInfo validatingInfo = null)
        {
            Guard.IsNotNull(message, nameof(message));

            Validate(message, validatingInfo);

            await MessageRepository.AddOrUpdateAsync(message);

            return message;
        }
    }
}
