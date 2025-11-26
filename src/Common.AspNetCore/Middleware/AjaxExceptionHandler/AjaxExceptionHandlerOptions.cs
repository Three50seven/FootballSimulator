using System;

namespace Common.AspNetCore
{
    public class AjaxExceptionHandlerOptions
    {
        /// <summary>
        /// Whether to include <see cref="Exception.Message"/> in the response message.
        /// Typically, this will be true in development and false in production.
        /// Default value is false.
        /// </summary>
        public bool IncludeExceptionMessage { get; set; } = false;
    }
}
