using Microsoft.AspNetCore.Mvc.Filters;
using Common.Core;
using System.Threading.Tasks;

namespace Common.AspNetCore.Mvc
{
    /// <summary>
    /// Filter to encodes all string properties on action argument models for POST requests.
    /// </summary>
    public class EncodeInputsActionFilter : IAsyncActionFilter
    {
        private readonly IContentEncoder _contentEncoder;

        public EncodeInputsActionFilter(IContentEncoder contentEncoder)
        {
            _contentEncoder = contentEncoder;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            if (context?.HttpContext?.Request?.Method == "POST")
            {
                foreach (var arg in context.ActionArguments)
                {
                    arg.ApplyToProperties<string>((propertyInfo, stringValue) =>
                    {
                        return _contentEncoder.Encode(stringValue);
                    });
                }
            }

            await next();
        }
    }
}
