using System;

namespace Common.Core.Domain
{
    public class MessageAddress : ValueObject<MessageAddress>, ICopyable<MessageAddress>
    {
        public static MessageAddress Empty => new MessageAddress();

        protected MessageAddress() { }

        public MessageAddress(string email, string name = null)
        {
            Email = email.SetEmptyToNull() ?? throw new ArgumentNullException(nameof(email));
            Name = name.SetEmptyToNull();
        }

        public string Email { get; private set; }
        public string Name { get; private set; }

        public override string ToString()
        {
            return Email;
        }

        public MessageAddress Copy()
        {
            return new MessageAddress(Email, Name);
        }
    }
}
