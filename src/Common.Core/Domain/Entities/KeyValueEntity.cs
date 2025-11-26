namespace Common.Core.Domain
{
    public abstract class KeyValueEntity : Entity<int>
    {
        protected KeyValueEntity() 
        { 
            Key = null!;
            Value = null!;
        }

        protected KeyValueEntity(string key, string value)
            : this(new KeyValuePair<string, string>(key, value))
        {

        }

        protected KeyValueEntity(KeyValuePair<string, string> keyValuePair)
        {
            Key = keyValuePair.Key.SetEmptyToNull();
            Value = keyValuePair.Value.SetEmptyToNull();
        }

        public string Key { get; private set; }
        public string Value { get; private set; }

        public KeyValuePair<string, object> ToKeyValuePair()
        {
            return new KeyValuePair<string, object>(Key, Value);
        }
    }
}
