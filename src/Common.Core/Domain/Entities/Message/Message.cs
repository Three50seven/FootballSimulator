using Common.Core.DTOs;
using Common.Core.Validation;
using System.Text;

namespace Common.Core.Domain
{
    /// <summary>
    /// Default entity to represent messages, notifications, and emails.
    /// </summary>
    public class Message : DomainEntity
    {
        protected Message() : base() 
        { 
            From = null!;
            Subject = null!;
            Content = null!;
            Category = null!;
            Error = null!;
        }

        public Message(
            MessageSendingInfo sendingInfo, 
            string content, 
            MessageOptions options = null!)
            : this(sendingInfo,
                   content,
                   categoryId: options?.CategoryId,
                   sendIndividually: options?.SendIndividually ?? false)
        {

        }

        public Message(
            string to,
            string subject,
            string content,
            int? categoryId = null,
            string cc = null!,
            string bcc = null!,
            string from = null!,
            string replyTo = null!,
            bool sendIndividually = false,
            bool isHtml = true,
            IEnumerable<int> attachmentDocumentIds = null!)
            : this (new MessageSendingInfo(to, subject, cc, bcc, from, replyTo),
                    content,
                    categoryId,
                    sendIndividually,
                    isHtml,
                    attachmentDocumentIds)
        {

        }

        public Message(
            MessageSendingInfo sendingInfo,
            string content,
            int? categoryId = null,
            bool sendIndividually = false,
            bool isHtml = true,
            IEnumerable<int> attachmentDocumentIds = null!)
            : this(sendingInfo?.Tos ?? Enumerable.Empty<MessageAddress>(), 
                  sendingInfo?.Subject ?? string.Empty, 
                  content,
                  categoryId: categoryId,
                  ccs: sendingInfo?.Ccs ?? Enumerable.Empty<MessageAddress>(),
                  bccs: sendingInfo?.Bccs ?? Enumerable.Empty<MessageAddress>(),
                  from: sendingInfo?.From!,
                  replyTos: sendingInfo?.ReplyTos ?? Enumerable.Empty<MessageAddress>(),
                  sendIndividually: sendIndividually,
                  isHtml: isHtml,
                  attachmentDocumentIds: attachmentDocumentIds)
        {

        }

        public Message(
            MessageAddress to,
            string subject,
            string content,
            int? categoryId = null,
            MessageAddress cc = null!,
            MessageAddress bcc = null!,
            MessageAddress from = null!,
            MessageAddress replyTo = null!,
            bool sendIndividually = false,
            bool isHtml = true,
            IEnumerable<int> attachmentDocumentIds = null!)
            : this(to == null! ? null : new MessageAddress[1] { to },
                    subject,
                    content,
                    categoryId,
                    cc == null! ? null : new MessageAddress[1] { cc },
                    bcc == null! ? null : new MessageAddress[1] { bcc },
                    from,
                    replyTo == null! ? null : new MessageAddress[1] { replyTo },
                    sendIndividually,
                    isHtml,
                    attachmentDocumentIds)
        {

        }

        public Message(
            IEnumerable<MessageAddress>? tos,
            string subject,
            string content,
            int? categoryId = null,
            IEnumerable<MessageAddress>? ccs = null!,
            IEnumerable<MessageAddress>? bccs = null!,
            MessageAddress from = null!,
            IEnumerable<MessageAddress>? replyTos = null!,
            bool sendIndividually = false,
            bool isHtml = true,
            IEnumerable<int> attachmentDocumentIds = null!)
        {
            Subject = subject.SetEmptyToNull();
            Content = content.SetEmptyToNull();
            CategoryId = categoryId ?? MessageCategory.DefaultId;
            From = from;
            CreateDate = DateTime.UtcNow;
            SendIndividually = sendIndividually;
            IsHtml = isHtml;

            if (tos != null)
            {
                foreach (var to in tos)
                {
                    AddRecipient(to);
                }
            }

            if (replyTos != null)
            {
                foreach (var replyTo in replyTos)
                {
                    AddReplyTo(replyTo);
                }
            }

            if (!SendIndividually)
            {
                if (ccs != null)
                {
                    foreach (var address in ccs)
                    {
                        AddRecipient(address, MessageRecipientTypeOption.Cc);
                    }
                }

                if (bccs != null)
                {
                    foreach (var address in bccs)
                    {
                        AddRecipient(address, MessageRecipientTypeOption.Bcc);
                    }
                }
            }

            if (attachmentDocumentIds != null)
            {
                foreach (var docId in attachmentDocumentIds)
                {
                    if (Attachments is List<MessageAttachment> list)
                        list.Add(new MessageAttachment(this, docId));
                }
            }
        }

        public MessageAddress From { get; protected set; }
        public IEnumerable<MessageReplyAddress> ReplyTos { get; protected set; } = new List<MessageReplyAddress>();
        public virtual IEnumerable<MessageAddress> ReplyToAddresses => ReplyTos?.Select(r => r.Address)!;

        public string Subject { get; protected set; }
        public string Content { get; protected set; }

        /// <summary>
        /// Setting this to true will send message to each To recipient individually instead of a combined To list. NOTE: This will disable all CC's.
        /// </summary>
        public bool SendIndividually { get; protected set; }

        public int CategoryId { get; protected set; }

        public virtual MessageCategory? Category { get; protected set; }

        public string? Error { get; protected set; }

        public DateTime CreateDate { get; protected set; }
        public DateTime? ProcessedDate { get; protected set; }
        public bool Sent { get; protected set; }
        public bool IsHtml { get; protected set; }

        public virtual IEnumerable<MessageRecipient> Recipients { get; private set; } = new List<MessageRecipient>();

        public virtual IEnumerable<MessageRecipient> ToRecipients => Recipients?.Where(r => r.TypeOption == MessageRecipientTypeOption.To)!;

        public virtual IEnumerable<MessageAddress> ToAddresses => ToRecipients?.Select(r => r.Address)!;

        public virtual IEnumerable<MessageAddress> CcAddresses => Recipients?.Where(r => r.TypeOption == MessageRecipientTypeOption.Cc)
                                                                             .Select(r => r.Address)!;

        public virtual IEnumerable<MessageAddress> BccAddresses => Recipients?.Where(r => r.TypeOption == MessageRecipientTypeOption.Bcc)
                                                                              .Select(r => r.Address)!;

        public virtual IEnumerable<MessageAttachment> Attachments { get; private set; } = new List<MessageAttachment>();

        public virtual IEnumerable<string> AttachmentPaths => Attachments?.Select(a => a.Document?.FullPath)!;

        public virtual void Complete()
        {
            ProcessedDate = DateTime.UtcNow;
            Sent = true;

            if (!SendIndividually)
            {
                foreach (var recipient in Recipients)
                {
                    recipient.Complete(ProcessedDate.Value);
                }
            }
        }

        public virtual void Fail(string message)
        {
            ProcessedDate = DateTime.UtcNow;
            Error = message;
            Sent = false;

            if (!SendIndividually)
            {
                foreach (var recipient in Recipients)
                {
                    recipient.Fail(message, ProcessedDate.Value);
                }
            }
        }

        public virtual void AddRecipient(
            MessageAddress address, 
            MessageRecipientTypeOption type = MessageRecipientTypeOption.To)
        {
            Guard.IsNotNull(address, nameof(address));

            if (Recipients is List<MessageRecipient> list)
                list.AddUnique(new MessageRecipient(this, address, type),
                    r => r.Address?.Email == address.Email && r.TypeOption == type);
        }

        public virtual void AddReplyTo(MessageAddress address)
        {
            Guard.IsNotNull(address, nameof(address));

            if (ReplyTos is List<MessageReplyAddress> list)
                list.AddUnique(new MessageReplyAddress(this, address),
                                                              mra => mra.Address?.Email == address.Email);
        }

        public virtual void Validate(MessageValidatingInfo validatingInfo)
        {
            if (validatingInfo == null!)
                return;

            // set a valid From if not set
            // NOTE: copying values in case EF is used and limitations are present
            //       in re-using same valueobject across multiple Messages
            if (From == null! || string.IsNullOrWhiteSpace(From.Email))
                From = validatingInfo.SystemAddress.Copy();

            if (validatingInfo.TestMode)
            {
                Subject = validatingInfo.BuildTestModeSubject(this);

                if (Recipients is List<MessageRecipient> recipList)
                    recipList.Clear();

                if (ReplyTos is List<MessageReplyAddress> replyList)
                    replyList.Clear();

                foreach (var to in validatingInfo.TestModeSendTos)
                {
                    // NOTE: copying values in case EF is used and limitations are present
                    //       in re-using same valueobject across multiple Messages
                    AddRecipient(to.Copy());
                }
            }
        }

        public virtual bool HasError => !string.IsNullOrWhiteSpace(Error);

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Subject: {Subject}");
            sb.AppendLine($"From: {From}");

            if (ToAddresses != null)
                sb.AppendLine($"To: {string.Join(",", ToAddresses)}");

            if (CcAddresses != null)
                sb.AppendLine($"Cc: {string.Join(",", CcAddresses)}");

            if (BccAddresses != null)
                sb.AppendLine($"Bcc: {string.Join(",", BccAddresses)}");

            sb.AppendLine($"CategoryId: {CategoryId}");
            sb.AppendLine($"Html?: {IsHtml.ToYesNoString()}");
            sb.AppendLine($"Send Individually?: {SendIndividually.ToYesNoString()}");

            return sb.ToString();
        }

        public static HashSet<string> GetAddressList(string addresses)
        {
            if (string.IsNullOrWhiteSpace(addresses))
                return new HashSet<string>();

            return new HashSet<string>(addresses.Replace(';', ',').Split(',')
                                                .Where(a => !string.IsNullOrWhiteSpace(a))
                                                .Select(a => a.Trim()));
        }
    }
}
