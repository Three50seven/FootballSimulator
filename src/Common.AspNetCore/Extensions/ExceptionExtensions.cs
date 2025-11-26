using Common.Core;
using System;

namespace Common.AspNetCore
{
    public static class ExceptionExtensions
    {
        public static string UnexpectedError = "An unexpected error has occurred.";

        /// <summary>
        /// Returns <see cref="UnexpectedError"/> message or the friendly exception message if in debug mode.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        public static string GetAjaxResponseMessage(this Exception ex, bool debug = false)
        {
            if (ex == null)
                return UnexpectedError;

            if (debug)
                return $"Server Error - {ex.GetFriendlyMessage()}";
            else
                return UnexpectedError;
        }
    }
}
