namespace Common.Core.Domain
{
    public abstract class LookupEntity : Entity<int>, ILookupEntity, IEntity<int>, IEntity
    {
        public string Name { get; protected set; } = string.Empty;

        protected LookupEntity()
        {
        }

        protected LookupEntity(string name)
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

        public override string ToString()
        {
            return Name;
        }

        public virtual SelectItem ToSelectItem()
        {
            return new SelectItem(base.Id, Name);
        }
    }
}
