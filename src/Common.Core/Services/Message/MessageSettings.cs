using Common.Core.Domain;
using System;

namespace Common.Core.Services
{
    public class MessageSettings
    {
        public MessageSendingOption SendType { get; set; } = MessageSendingOption.Smtp;
        public MessageQueueOption QueueType { get; set; } = MessageQueueOption.DatabaseOnly;
        public MessageSmtpSettings Smtp { get; set; } = new MessageSmtpSettings();
        public bool TestMode { get; set; } = true;
        public string TestModeSendTo { get; set; }
        public string SystemEmail { get; set; }
        public string SystemEmailName { get; set; }
        public string AttachmentsDirectory { get; set; }

        public virtual MessageValidatingInfo ToValidatingInfo(string testModeSendTo = null)
        {
            if (string.IsNullOrWhiteSpace(SystemEmail))
                throw new ArgumentNullException($"{nameof(SystemEmail)} required on {typeof(MessageSettings).Name} configuration.");

            return new MessageValidatingInfo(new MessageAddress(SystemEmail, SystemEmailName), 
                                             TestMode,
                                             testModeSendTo ?? TestModeSendTo);
        } 
    }

    public enum MessageQueueOption
    {
        DatabaseOnly,
        DirectSend,
        QueueBackgroundTask
    }

    public enum MessageSendingOption
    {
        Smtp,
        WriteToFile,
    }
}
