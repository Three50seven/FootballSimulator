namespace Common.Core.Domain
{
    public class MessageRecipient : Entity<long>
    {
        protected MessageRecipient() { }

        public MessageRecipient(
            Message message, 
            string email, 
            string name = null!,
            MessageRecipientTypeOption type = MessageRecipientTypeOption.To)
            : this (message, new MessageAddress(email, name), type)
        {
            
        }

        public MessageRecipient(
            Message message,
            MessageAddress address,
            MessageRecipientTypeOption type = MessageRecipientTypeOption.To)
        {
            Message = message;
            TypeId = (int)type;
            Address = address ?? throw new ArgumentNullException(nameof(address));
        }

        public MessageRecipient(
            int messageId, 
            string email, 
            string name = null!,
            MessageRecipientTypeOption type = MessageRecipientTypeOption.To)
            : this (messageId, new MessageAddress(email, name), type)
        {

        }

        public MessageRecipient(
            int messageId,
            MessageAddress address,
            MessageRecipientTypeOption type = MessageRecipientTypeOption.To)
        {
            MessageId = messageId;
            TypeId = (int)type;
            Address = address ?? throw new ArgumentNullException(nameof(address));
        }

        public int MessageId { get; private set; }

        public Message? Message { get; private set; }

        public int TypeId { get; private set; }

        public MessageRecipientTypeOption TypeOption => (MessageRecipientTypeOption)TypeId;

        public MessageRecipientType? Type { get; private set; }

        public MessageAddress? Address { get; private set; }
        public DateTime? ProcessedDate { get; private set; }
        public bool Sent { get; private set; }
        public string? Error { get; private set; }

        public virtual void Complete(DateTime? date = null)
        {
            ProcessedDate = date ?? DateTime.UtcNow;
            Sent = true;
        }

        public virtual void Fail(string message, DateTime? date = null)
        {
            ProcessedDate = date ?? DateTime.UtcNow;
            Error = message;
            Sent = false;
        }
    }
}
