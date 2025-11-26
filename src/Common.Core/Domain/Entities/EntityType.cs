using System;

namespace Common.Core.Domain
{
    /// <summary>
    /// Lookup entity for domain's list of domain entity types 
    /// that should be tracked in other areas (history, etc.).
    /// </summary>
    public class EntityType : LookupEntity
    {
        private EntityType() { }
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
