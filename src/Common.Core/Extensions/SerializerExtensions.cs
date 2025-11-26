namespace Common.Core
{
    public static class SerializerExtensions
    {
        public static T Deserialize<T>(this ISerializer serializer, string serializedValue) 
            where T : class
        {
            return (T)serializer.Deserialize(serializedValue, typeof(T));
        }
    }
}
