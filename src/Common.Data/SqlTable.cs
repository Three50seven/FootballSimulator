using Humanizer;
using System.Reflection;
using System.Text;

namespace Common.Data
{
    public class SqlTable<T> : SqlTable where T : class
    {
        public SqlTable()
            : base(typeof(T))
        {
        }

        public SqlTable(string tableName)
            : base(typeof(T), tableName)
        {
        }

        public SqlTable(bool underscore)
            : base(typeof(T), underscore)
        {
        }

        public SqlTable(bool underscore, string tableName)
            : base(typeof(T), underscore, tableName)
        {
        }
    }

    public class SqlTable
    {
        private readonly List<KeyValuePair<string, Type>> _fields = [];
        private static readonly Dictionary<Type, string> _dataTypeMapper = new()
        {
            // Add the rest of your CLR Types to SQL Types mapping here
            { typeof(int), "INT" },
            { typeof(string), "NVARCHAR(MAX)" },
            { typeof(bool), "BIT" },
            { typeof(DateTime), "DATETIME" },
            { typeof(float), "FLOAT" },
            { typeof(decimal), "DECIMAL(18,0)" },
            { typeof(Guid), "UNIQUEIDENTIFIER" }
        };

        public SqlTable(Type type)
            : this(type, null)
        {

        }

        public SqlTable(Type type, string tableName)
            : this(type, true, tableName)
        {

        }

        public SqlTable(Type type, bool underscore)
            : this(type, underscore, null)
        {

        }

        public SqlTable(Type type, bool underscore, string tableName)
        {
            Name = string.IsNullOrWhiteSpace(tableName) ? type.Name : tableName;

            foreach (PropertyInfo p in type.GetProperties())
            {
                _fields.Add(new KeyValuePair<string, Type>(underscore ? p.Name.Underscore() : p.Name, p.PropertyType));
            }

            CreationScript = BuildCreateScript();
        }

        public string Name { get; private set; }

        public string CreationScript { get; private set; }

        protected virtual string BuildCreateScript()
        {
            var scriptBuilder = new StringBuilder();

            scriptBuilder.AppendLine($"CREATE TABLE [{Name}]");
            scriptBuilder.AppendLine("(");

            for (int i = 0; i < _fields.Count; i++)
            {
                KeyValuePair<string, Type> field = _fields[i];

                if (!_dataTypeMapper.TryGetValue(field.Value, out string value))
                    throw new KeyNotFoundException($"No available SQL data type mapping for type {field.Value.FullName}.");

                scriptBuilder.Append($"\t {field.Key} {value}");

                if (i != _fields.Count - 1)
                    scriptBuilder.Append(',');

                scriptBuilder.Append(Environment.NewLine);
            }

            scriptBuilder.AppendLine(")");

            return scriptBuilder.ToString();
        }
    }
}
