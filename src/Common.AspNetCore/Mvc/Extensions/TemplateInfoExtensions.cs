using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Common.Core;

namespace Common.AspNetCore.Mvc
{
    public static class TemplateInfoExtensions
    {
        public static string GetFullHtmlFieldId(this TemplateInfo templateInfo, string name)
        {
            return templateInfo?.GetFullHtmlFieldName(name.SetNullToEmpty())?.Replace(".", "_");
        }
    }
}
