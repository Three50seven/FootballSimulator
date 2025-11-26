using Common.Core;
using System;

namespace Common.AspNetCore
{
    public class ErrorInfo
    {
        public static string DefaultFriendlyMessage = "An unexpected error occurred while processing your request. We apologize for this inconvenience.";

        public ErrorInfo(Exception ex, string path, Guid? eventId)
        {
            Exception = ex;
            Path = path;
            Message = null;

            if (eventId.HasValue())
                EventId = (Guid)eventId;
            else
                EventId = Guid.Empty;
        }

        public ErrorInfo(string message, Guid? eventId = null)
        {
            Message = message.SetNullToEmpty(trim: true);

            if (eventId.HasValue())
                EventId = (Guid)eventId;
            else
                EventId = Guid.Empty;
        }

        public ErrorInfo(Guid? eventId) 
            : this(null, eventId)
        {
        }

        public Exception Exception { get; private set; }
        public string Path { get; private set; }
        public string Message { get; private set; }
        public Guid EventId { get; private set; }
        public virtual bool DisplayCustomMessage => !string.IsNullOrWhiteSpace(Message);

        public virtual string ErrorCode
        {
            get
            {
                if (EventId == Guid.Empty)
                    return string.Empty;

                var eventId = EventId.ToString().Replace("-", "");
                return eventId[^9..].ToUpper();
            }
        }

        public FriendlyErrorInfo ToFriendly()
        {
            return new FriendlyErrorInfo(Message, ErrorCode);
        }
    }
}
