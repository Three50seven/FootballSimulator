using System.Net.Mail;

namespace Common.Core.Domain
{
    public static class MessageAddressExtensions
    {
        /// <summary>
        /// Convert MessageAddress to System.Net.Mail item MailAddress.
        /// </summary>
        /// <param name="messageAddress"></param>
        /// <returns></returns>
        public static MailAddress ToMailAddress(this MessageAddress messageAddress)
        {
            if (messageAddress == null)
                return null;

            // NOTE: this check may not be necessary
            if (string.IsNullOrWhiteSpace(messageAddress.Name))
                return new MailAddress(messageAddress.Email);
            else
                return new MailAddress(messageAddress.Email, messageAddress.Name);
        }
    }
}
