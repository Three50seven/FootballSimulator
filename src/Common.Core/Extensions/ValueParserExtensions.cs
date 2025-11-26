namespace Common.Core
{
    public static class ValueParserExtensions
    {
        public static T Parse<T>(this IValueParser valueParser, string value)
        {
            return (T)valueParser.Parse(value, typeof(T));
        }

        public static bool TryParse<T>(this IValueParser valueParser, string value, out T parsedValue)   
        {
            var parseResult = valueParser.TryParse(value, typeof(T), out object parsedObject);
            parsedValue = parseResult ? (T)parsedObject : default(T);
         
            return parseResult;
        }
    }
}
