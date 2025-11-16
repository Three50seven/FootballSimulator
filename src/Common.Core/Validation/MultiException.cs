namespace Common.Core
{
    [Serializable]
    public sealed class MultiException : Exception
    {
        private Exception[] _innerExceptions;

        public IEnumerable<Exception> InnerExceptions
        {
            get
            {
                if (_innerExceptions != null)
                {
                    int i = 0;
                    while (i < _innerExceptions.Length)
                    {
                        yield return _innerExceptions[i];
                        int num = i + 1;
                        i = num;
                    }
                }
            }
        }

        public MultiException()
            : base() => _innerExceptions = [];

        public MultiException(string? message)
            : base(message) => _innerExceptions = [];

        public MultiException(string message, Exception innerException)
            : base(message, innerException)
        {
            _innerExceptions = [innerException];
        }

        public MultiException(IEnumerable<Exception> innerExceptions)
            : this(null, innerExceptions)
        {
        }

        public MultiException(Exception[] innerExceptions)
            : this(null, (IEnumerable<Exception>)innerExceptions)
        {
        }

        public MultiException(string? message, Exception[] innerExceptions)
            : this(message, (IEnumerable<Exception>)innerExceptions)
        {
        }

        public MultiException(string? message, IEnumerable<Exception> innerExceptions)
            : base(message, innerExceptions.FirstOrDefault())
        {
            if (innerExceptions.AnyNull())
            {
                throw new ArgumentNullException("One or more inner exception is null.");
            }

            _innerExceptions = [.. innerExceptions];
        }
    }
}
