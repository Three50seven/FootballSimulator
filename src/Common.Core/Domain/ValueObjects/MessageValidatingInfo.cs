using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Core.Domain
{
    public class MessageValidatingInfo : ValueObject<MessageValidatingInfo>
    {
        protected MessageValidatingInfo() { }

        public MessageValidatingInfo(
            MessageAddress systemAddress,
            bool testMode,
            MessageAddress testModeSendTo)
            : this(systemAddress,
                   testMode,
                   testModeSendTo == null ? null : new List<MessageAddress>() { testModeSendTo })
        {

        }

        public MessageValidatingInfo(
            MessageAddress systemAddress,
            bool testMode,
            string testModeSendTos)
            : this(systemAddress,
                   testMode,
                   Message.GetAddressList(testModeSendTos).Select(e => new MessageAddress(e)))
        {

        }

        public MessageValidatingInfo(
            MessageAddress systemAddress,
            bool testMode,
            IEnumerable<MessageAddress> testModeSendTos)
        {
            SystemAddress = systemAddress ?? throw new ArgumentNullException(nameof(systemAddress));
            TestMode = testMode;

            if (testMode && !testModeSendTos.HasItems())
                throw new ArgumentNullException(nameof(testModeSendTos));

            TestModeSendTos = testModeSendTos ?? new List<MessageAddress>();
        }

        public MessageAddress SystemAddress { get; private set; }
        public bool TestMode { get; private set; }
        public IEnumerable<MessageAddress> TestModeSendTos { get; private set; } = new List<MessageAddress>(); 

        /// <summary>
        /// Construct custom message for the Subject value to be used when system is under Test Mode via <see cref="TestMode"/>.
        /// Defaults to oringal subject + "(TEST MODE)" + addresses
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual string BuildTestModeSubject(Message message)
        {
            if (message == null)
                return string.Empty;

            if (!message.Recipients.HasItems())
                return message.Subject;

            var sb = new StringBuilder();

            sb.Append(message.Subject);

            sb.Append(" (TEST MODE) ");

            if (message.SendIndividually)
                sb.Append("(sent individually) ");

            string to = string.Join(",", message.ToAddresses.Select(a => a.Email));
            string cc = string.Join(",", message.CcAddresses.Select(a => a.Email));
            string bcc = string.Join(",", message.BccAddresses.Select(a => a.Email));

            if (!string.IsNullOrWhiteSpace(to))
                sb.Append($"To: {to} ");

            if (!string.IsNullOrWhiteSpace(cc))
                sb.Append($"CC: {cc} ");

            if (!string.IsNullOrWhiteSpace(bcc))
                sb.Append($"BCC: {bcc} ");

            return sb.ToString().Trim().Truncate(500, "...");
        }
    }
}
