namespace Common.Core.Domain
{
    public class SelectItem : ValueObject<SelectItem>
    {
        public static readonly SelectItem Default = new();

        public virtual string Value { get; protected set; } = string.Empty;

        public virtual string Name { get; protected set; } = string.Empty;

        public bool Selected { get; set; } = false;


        public int? Id { get; protected set; }

        protected SelectItem()
        {
        }

        public SelectItem(string value)
            : this(value, value)
        {
        }

        public SelectItem(int id)
            : this(id, id.ToString())
        {
        }

        public SelectItem(string value, string name)
        {
            Value = value.SetEmptyToNull() ?? string.Empty;
            Name = name.SetEmptyToNull() ?? string.Empty;
            Id = (string.IsNullOrWhiteSpace(Value) ? null : new int?(Value.ParseInteger(allowEmpty: true, throwError: false)));
        }

        public SelectItem(int id, string name)
        {
            Value = id.ToString();
            Name = name.SetEmptyToNull() ?? string.Empty;
            Id = id;
        }

        public SelectItem(Enum enumValue)
            : this(Convert.ToInt32(enumValue), enumValue.AsFriendlyName())
        {
        }

        public SelectItem Copy()
        {
            return new SelectItem(Value, Name);
        }
    }
}
