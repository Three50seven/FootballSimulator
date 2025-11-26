namespace Common.Core.Domain
{
    /// <summary>
    /// Standard base implementation for simple "lookup" entities that have and Id and Name.
    /// </summary>
    public abstract class LookupEntity : Entity<int>, ILookupEntity
    {
        protected LookupEntity() 
        { 
            Name = string.Empty;
        }

        protected LookupEntity(string name) 
            : base()
        {
            Name = name.Trim();
        }

        protected LookupEntity(int id, string name) 
            : base(id)
        {
            Name = name.Trim();
        }

        protected LookupEntity(Enum value) 
            : this(value.ToInt(), value.AsFriendlyName())
        {

        }

        public string Name { get; protected set; }

        public override string ToString()
        {
            return Name;
        }

        public virtual SelectItem ToSelectItem()
        {
            return new SelectItem(Id, Name);
        }
    }
}
