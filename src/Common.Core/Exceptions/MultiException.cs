using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Core
{
    [Serializable]
    public sealed class MultiException : Exception
    {
        private Exception[] innerExceptions;

        public IEnumerable<Exception> InnerExceptions
        {
            get
            {
                if (this.innerExceptions != null)
                {
                    for (int i = 0; i < this.innerExceptions.Length; ++i)
                    {
                        yield return this.innerExceptions[i];
                    }
                }
            }
        }

        public MultiException()
            : base()
        {
        }

        public MultiException(string message)
            : base()
        {
        }

        public MultiException(string message, Exception innerException)
            : base(message, innerException)
        {
            this.innerExceptions = new Exception[1] { innerException };
        }

        public MultiException(IEnumerable<Exception> innerExceptions)
            : this(null, innerExceptions)
        {
        }

        public MultiException(Exception[] innerExceptions)
            : this(null, (IEnumerable<Exception>)innerExceptions)
        {
        }

        public MultiException(string message, Exception[] innerExceptions)
            : this(message, (IEnumerable<Exception>)innerExceptions)
        {
        }

        public MultiException(string message, IEnumerable<Exception> innerExceptions)
            : base(message, innerExceptions.FirstOrDefault())
        {
            if (innerExceptions.AnyNull())
                throw new ArgumentNullException("One or more inner exception is null.");

            this.innerExceptions = innerExceptions.ToArray();
        }
    }
}
