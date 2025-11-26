using System;

namespace Common.Core.Domain
{
    public class MessageSendResult
    {
        public MessageSendResult(Guid messageId, bool success, string message, Exception ex = null)
        {
            MessageId = messageId;
            Succeeded = success;
            Message = message;
            Exception = ex;
        }

        public Guid MessageId { get; private set; }
        public bool Succeeded { get; private set; }
        public string Message { get; private set; }
        public Exception Exception { get; private set; }
        public string ErrorMessage => Succeeded ? string.Empty : $"Message '{MessageId}' failed to send: {Message}";

        public static MessageSendResult Success(Guid messageId)
        {
            return new MessageSendResult(messageId, true, "Message(s) sent successfully.");
        }

        public static MessageSendResult Error(Guid messageId, string error)
        {
            return new MessageSendResult(messageId, false, error);
        }

        public static MessageSendResult Error(Guid messageId, Exception ex)
        {
            return new MessageSendResult(messageId, false, ex?.GetFriendlyMessage(), ex);
        }
    }
}
