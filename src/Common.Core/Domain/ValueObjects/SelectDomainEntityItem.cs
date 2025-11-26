using System;

namespace Common.Core.Domain
{
    public class SelectDomainEntityItem : SelectItem
    {
        protected SelectDomainEntityItem() : base() { }

        public SelectDomainEntityItem(string value, string name, Guid guid) 
            : base(value, name)
        {
            Guid = guid;
        }

        public SelectDomainEntityItem(int id, string name, Guid guid) 
            : base(id, name)
        {
            Guid = guid;
        }

        public Guid Guid { get; protected set; }
    }
}
