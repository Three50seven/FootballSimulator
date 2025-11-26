namespace Common.Core.Domain
{
    public class MessageAttachment : IEntity
    {
        protected MessageAttachment() { }

        public MessageAttachment(Message message, Document document)
        {
            MessageId = message.Id;
            Message = message;
            DocumentId = document.Id;
            Document = document;
        }

        public MessageAttachment(Message message, int documentId)
        {
            MessageId = message.Id;
            Message = message;
            DocumentId = documentId;
        }

        public MessageAttachment(int messageId, int documentId)
        {
            MessageId = messageId;
            DocumentId = documentId;
        }

        public int MessageId { get; private set; }
        public virtual Message? Message { get; private set; }

        public int DocumentId { get; private set; }
        public virtual Document? Document { get; private set; }
    }
}
