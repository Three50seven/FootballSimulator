namespace Common.Core.Domain
{
    public class TrackableInfo : ValueObject<TrackableInfo>
    {
        protected TrackableInfo() { }

        public TrackableInfo(string value, string description)
        {
            Value = value;
            Description = description;
        }

        public TrackableInfo(int id, string description)
            : this(id.ToString(), description)
        {

        }

        public string Value { get; private set; }
        public string Description { get; private set; }
    }
}
