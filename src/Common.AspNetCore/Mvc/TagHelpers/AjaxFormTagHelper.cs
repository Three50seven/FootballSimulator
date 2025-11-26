using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Common.AspNetCore.Mvc.TagHelpers
{
    [HtmlTargetElement("form", Attributes = FormAttribute)]
    public class AjaxFormTagHelper : TagHelper
    {
        public const string FormAttribute = "ajax-form";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("data-ajax", "true");
            output.Attributes.Add("data-ajax-method", "POST");
            output.Attributes.Add("data-ajax-success", "MODULES.Ajax.OnSuccess");

            output.Attributes.Remove(new TagHelperAttribute(FormAttribute));
        }
    }
}
