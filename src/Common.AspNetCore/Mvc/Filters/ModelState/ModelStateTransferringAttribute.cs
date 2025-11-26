using Microsoft.AspNetCore.Mvc.Filters;

namespace Common.AspNetCore.Mvc
{
    public abstract class ModelStateTransferringAttribute : ActionFilterAttribute
    {
        public static string Key = "ModelStateTransferring";
    }
}
