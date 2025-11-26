using Microsoft.Extensions.Logging;
using Common.Core.Domain;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Common.Core.Services
{
    /// <summary>
    /// Message sender that writes the contents of the message <see cref="Message"/> to text files.
    /// The directory path is set to the current directory in "messages_output" folder.
    /// Two files are generated: 1) raw content of the messages and 2) the metadata of the message (to, from, etc.)
    /// This sender class is used when <see cref="MessageSettings.SendType"/> is <see cref="MessageSendingOption.WriteToFile"/>.
    /// </summary>
    public class MessageSaveToFileSender : MessageSenderBase<MessageSaveToFileSender>
    {
        public static string DirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "messages_output");

        static MessageSaveToFileSender()
        {
            Directory.CreateDirectory(DirectoryPath);
        }

        public MessageSaveToFileSender(ILogger<MessageSaveToFileSender> logger) 
            : base(logger)
        {
            
        }

        protected override void OnSend(Message message)
        {
            string fileNamePrefix = $"message_{DateTime.Now.ToString("yyyyMMddTHHmmss")}_{message.Guid.ToString().Replace("-", "").ToLower()}";

            File.WriteAllText(Path.Combine(DirectoryPath, $"{fileNamePrefix}_content.txt"), message.Content);
            File.WriteAllText(Path.Combine(DirectoryPath, $"{fileNamePrefix}_metadata.txt"), message.ToString());

            if (message.SendIndividually)
            {
                foreach (var recipient in message.ToRecipients)
                {
                    recipient.Complete();
                }
            }
        }

        protected override Task OnSendAsync(Message message)
        {
            OnSend(message);
            return Task.CompletedTask;
        }
    }
}
