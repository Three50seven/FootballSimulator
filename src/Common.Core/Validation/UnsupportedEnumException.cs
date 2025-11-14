namespace Common.Core
{
    [Serializable]
    public class UnsupportedEnumException : Exception
    {
        public UnsupportedEnumException(Enum @enum)
            : this($"The enum value {@enum} of type {@enum.GetType().FullName} is not supported in the current operation.")
        {
        }

        public UnsupportedEnumException(string message)
            : base(message)
        {
        }

        public UnsupportedEnumException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
