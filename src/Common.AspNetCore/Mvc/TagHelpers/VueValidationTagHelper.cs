using Microsoft.AspNetCore.Razor.TagHelpers;
using Common.Core.Validation;

namespace Common.AspNetCore.Mvc.TagHelpers
{
    [HtmlTargetElement("span", Attributes = "vue-validation-for")]
    public class VueValidationTagHelper : TagHelper
    {
        [HtmlAttributeName("vue-validation-for")]
        public string Name { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            Guard.IsNotNull(Name, nameof(Name));
            
            output.Attributes.SetAttribute("v-bind:data-valmsg-for", Name);
            output.Attributes.SetAttribute("data-valmsg-replace", "true");
            output.Attributes.SetAttribute("class", "field-validation-valid input-validation-error-message");
        }
    }
}
