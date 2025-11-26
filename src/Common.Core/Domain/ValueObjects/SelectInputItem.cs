using System;

namespace Common.Core.Domain
{
    public class SelectInputItem
    {
        public SelectInputItem()
        { }

        public SelectInputItem(string value, string name) : this(value, name, false)
        { }

        public SelectInputItem(int id, string name) : this(id.ToString(), name)
        { }

        public SelectInputItem(Enum enumValue) : this(enumValue.ToInt(), enumValue.AsFriendlyName())
        {

        }

        public SelectInputItem(int id, string name, bool selected) : this(id.ToString(), name, selected)
        { }

        public SelectInputItem(string value, string name, bool selected)
        {
            Value = value;
            Name = name;
            Selected = selected;
        }

        public virtual string Value { get; set; }
        public virtual string Name { get; set; }
        public virtual bool Selected { get; set; }

        public virtual int Id => string.IsNullOrWhiteSpace(Value) ? 0 : Value.ParseInteger(allowEmpty: true, throwError: false);
    }
}
