using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Common.AspNetCore.Mvc.TagHelpers
{
    [HtmlTargetElement("script", Attributes = "external-ref")]
    public class ExternalReferenceScriptTagHelper : TagHelper
    {
        [HtmlAttributeName("external-ref")]
        public ExternalContentReference ExternalContentReference { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.ApplyExternalReference(ExternalContentReference);
        }
    }
}
