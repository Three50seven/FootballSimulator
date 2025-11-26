using Microsoft.Data.SqlClient;

namespace Common.Data
{
    public static class SqlParameterExtensions
    {
        public static ICollection<SqlParameter> AddArrayParameter<T>(
            this ICollection<SqlParameter> paramsDefinition,
            T[] parameters,
            string paramName,
            out string cmd)
        {
            cmd = string.Empty;

            if (parameters == null || parameters.Length == 0)
                return paramsDefinition;

            if (string.IsNullOrWhiteSpace(paramName))
                throw new ArgumentNullException(nameof(paramName));

            if (!paramName.StartsWith('@'))
                paramName = "@" + paramName;

            var cmdParameters = new string[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                cmdParameters[i] = $"{paramName}{i}";
                paramsDefinition.Add(new SqlParameter(cmdParameters[i], parameters[i]));
            }

            cmd = string.Join(", ", cmdParameters);

            return paramsDefinition;
        }
    }
}
