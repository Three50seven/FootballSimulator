using Microsoft.AspNetCore.Http;
using System;

namespace Common.AspNetCore
{
    public class ExceptionLoggingOptions
    {
        /// <summary>
        /// Function callback to allow custom evaluation of an exception with the current request 
        /// to determine if the exception should be logged.
        /// </summary>
        public Func<HttpContext, Exception, bool> ShouldLogException { get; set; }
    }
}
