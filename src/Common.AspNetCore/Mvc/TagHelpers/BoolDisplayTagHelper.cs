using Microsoft.AspNetCore.Razor.TagHelpers;
using Common.Core;

namespace Common.AspNetCore.Mvc.TagHelpers
{
    [HtmlTargetElement("span", Attributes = "bool-display-for")]
    public class BoolDisplayTagHelper : TagHelper
    {
        [HtmlAttributeName("bool-display-for")]
        public bool Value { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Content.SetContent(Value.ToYesNoString());
        }
    }
}
