using Common.Core.Validation;
using System;

namespace Common.Core
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Gets the most inner exception. If is <see cref="ValidationException"/>, 
        /// <see cref="ValidationException.FriendlyMessage"/> is returned, else <see cref="Exception.Message"/> is returned.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetFriendlyMessage(this Exception ex)
        {
            if (ex == null)
                return string.Empty;

            var trueException = GetInnerMostException(ex);

            if (trueException is ValidationException)
                return (trueException as ValidationException).FriendlyMessage;
            else
                return trueException.Message;
        }

        /// <summary>
        /// Returns inner exception if available. Recursively checks against inner exceptions to return the deepest exception that was thrown.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static Exception GetInnerMostException(this Exception ex)
        {
            if (ex == null)
                return null;

            return ex.InnerException != null ? GetInnerMostException(ex.InnerException) : ex;
        }
    }
}
