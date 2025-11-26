namespace Common.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExportAttribute : Attribute
    {
        public string? DisplayName { get; set; }
        public ExportPropertyType PropertyType { get; set; } = ExportPropertyType.Text;
        public string? Format { get; set; }
    }

    public enum ExportPropertyType
    {
        Text,
        Number,
        Date
    }
}
