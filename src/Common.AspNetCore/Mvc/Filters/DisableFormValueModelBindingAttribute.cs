using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Common.AspNetCore.Mvc
{
    /// <summary>
    /// Removes Value proivder factories from the request context.
    /// Non-file properties on the model may need to be rebound.
    /// Intended to allow for larger file uploads and prevent exception "System.IO.IOException - Unexpected end of Stream, the content may have already been read by another component".
    /// Reference - https://stackoverflow.com/a/49867344/9882811 and https://stackoverflow.com/a/40843715/9882811
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class DisableFormValueModelBindingAttribute : Attribute, IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            context.ValueProviderFactories.RemoveType<FormValueProviderFactory>();
            context.ValueProviderFactories.RemoveType<JQueryFormValueProviderFactory>();
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}
