namespace Common.Core.Domain
{
    public class EntityType : LookupEntity
    {
        private EntityType()
        {
        }

        public EntityType(Enum @enum)
            : base(@enum)
        {
        }

        public EntityType(int id, string name)
            : base(id, name)
        {
        }
    }
}
