namespace Common.Core.Domain
{
    /// <summary>
    /// Property bag of standard message/sending information (to, subject, etc.)
    /// </summary>
    public class MessageSendingInfo : ValueObject<MessageSendingInfo>
    {
        protected MessageSendingInfo() { }

        public MessageSendingInfo(
            string to, 
            string subject, 
            string cc = null!, 
            string bcc = null!, 
            string from = null!,
            string replyTo = null!)
            : this (Message.GetAddressList(to).Select(e => new MessageAddress(e)),
                    subject,
                    Message.GetAddressList(cc).Select(e => new MessageAddress(e)),
                    Message.GetAddressList(bcc).Select(e => new MessageAddress(e)),
                    string.IsNullOrWhiteSpace(from) ? null : new MessageAddress(from),
                    Message.GetAddressList(replyTo).Select(e => new MessageAddress(e)))
        {

        }

        public MessageSendingInfo(
            MessageAddress to,
            string subject,
            MessageAddress cc = null!,
            MessageAddress bcc = null!,
            MessageAddress from = null!,
            MessageAddress replyTo = null!)
            : this (to == null! ? null : new MessageAddress[1] { to },
                    subject,
                    cc == null! ? null : new MessageAddress[1] { cc },
                    bcc == null! ? null : new MessageAddress[1] { bcc },
                    from,
                    replyTo == null! ? null : new MessageAddress[1] { replyTo })
        {

        }

        public MessageSendingInfo(
            IEnumerable<MessageAddress>? tos,
            string subject,
            IEnumerable<MessageAddress>? ccs = null,
            IEnumerable<MessageAddress>? bccs = null,
            MessageAddress? from = null!,
            IEnumerable<MessageAddress>? replyTos = null!)
        {
            Tos = tos ?? throw new ArgumentNullException(nameof(tos));
            Subject = subject;
            Ccs = ccs;
            Bccs = bccs;
            From = from;
            ReplyTos = replyTos;
        }

        public IEnumerable<MessageAddress>? Tos { get; private set; }
        public IEnumerable<MessageAddress>? Ccs { get; private set; }
        public IEnumerable<MessageAddress>? Bccs { get; private set; }
        public MessageAddress? From { get; private set; }
        public string? Subject { get; private set; }
        public IEnumerable<MessageAddress>? ReplyTos { get; private set; }
    }
}
