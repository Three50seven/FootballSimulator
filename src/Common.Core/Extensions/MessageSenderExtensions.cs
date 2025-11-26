using Common.Core.Domain;
using Common.Core.Validation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Core
{
    public static class MessageSenderExtensions
    {
        /// <summary>
        /// Send multiple messages and return a results model.
        /// </summary>
        /// <param name="messageSender"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        public static BulkMessagesSendResult SendRange(this IMessageSender messageSender, IEnumerable<Message> messages)
        {
            Guard.IsNotNullOrEmptyList(messages, nameof(messages));

            var results = new List<MessageSendResult>();

            foreach (var message in messages)
            {
                results.Add(messageSender.Send(message));
            }

            return new BulkMessagesSendResult(results);
        }

        /// <summary>
        /// Send multiple messages and return a results model.
        /// </summary>
        /// <param name="messageSender"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        public static async Task<BulkMessagesSendResult> SendRangeAsync(this IMessageSender messageSender, IEnumerable<Message> messages)
        {
            Guard.IsNotNullOrEmptyList(messages, nameof(messages));

            var results = new List<MessageSendResult>();

            foreach (var message in messages)
            {
                results.Add(await messageSender.SendAsync(message));
            }

            return new BulkMessagesSendResult(results);
        }
    }
}
