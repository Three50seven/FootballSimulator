namespace Common.Core.Domain
{
    public class MessageRecipientType : LookupEntity
    {
        protected MessageRecipientType() { }
        
        public MessageRecipientType(MessageRecipientTypeOption type) : base (type) { }
    }
}
