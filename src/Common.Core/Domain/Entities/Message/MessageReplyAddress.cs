namespace Common.Core.Domain
{
    public class MessageReplyAddress : Entity<long>
    {
        protected MessageReplyAddress() { }

        public MessageReplyAddress(int messageId, MessageAddress address)
        {
            MessageId = messageId;
            Address = address ?? throw new ArgumentNullException(nameof(address));
        }

        public MessageReplyAddress(Message message, MessageAddress address)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Address = address ?? throw new ArgumentNullException(nameof(address));
        }

        public int MessageId { get; private set; }
        public Message? Message { get; private set; }

        public MessageAddress? Address { get; private set; }
    }
}
