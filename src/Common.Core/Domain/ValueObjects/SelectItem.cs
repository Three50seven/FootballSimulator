using System;

namespace Common.Core.Domain
{
    public class SelectItem : ValueObject<SelectItem>
    {
        public static readonly SelectItem Default = new SelectItem();

        protected SelectItem() { }

        public SelectItem(string value)
            : this(value, value) { }

        public SelectItem(int id)
            : this(id, id.ToString()) { }

        public SelectItem(string value, string name)
        {
            Value = value.SetEmptyToNull();
            Name = name.SetEmptyToNull();
            Id = string.IsNullOrWhiteSpace(Value) ? (int?)null : Value.ParseInteger(allowEmpty: true, throwError: false);
        }

        public SelectItem(int id, string name)
        {
            Value = id.ToString();
            Name = name.SetEmptyToNull();
            Id = id;
        }

        public SelectItem(Enum enumValue)
            : this(Convert.ToInt32(enumValue), enumValue.AsFriendlyName())
        {

        }

        public virtual string Value { get; protected set; }
        public virtual string Name { get; protected set; }
        public bool Selected { get; set; } = false;
        public int? Id { get; protected set; }

        public SelectItem Copy()
        {
            return new SelectItem(this.Value, this.Name);
        }
    }
}
