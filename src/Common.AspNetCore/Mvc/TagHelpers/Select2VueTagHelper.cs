using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Common.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace Common.AspNetCore.Mvc.TagHelpers
{
    /// <summary>
    /// Render select-2 tag to be used with custom Vue component "select-2".
    /// </summary>
    public static class Select2VueTagHelper
    {
        [HtmlTargetElement("select-2-vue", TagStructure = TagStructure.NormalOrSelfClosing)]
        public class Select2TagHelper : TagHelper
        {
            private readonly string _tagName = "select-2";
            private readonly string _defaultRequiredMessage = "This field is required.";

            public string Name { get; set; }

            [HtmlAttributeName("for")]
            public ModelExpression For { get; set; }

            [HtmlAttributeName("list")]
            public string Selections { get; set; }

            [HtmlAttributeName("multiple")]
            public bool Multiple { get; set; } = false;

            [HtmlAttributeName("required-message")]
            public string RequiredMessage { get; set; }

            [HtmlAttributeName("required")]
            public bool Required { get; set; }

            [HtmlAttributeName("disabled")]
            public bool Disabled { get; set; }

            public override void Process(TagHelperContext context, TagHelperOutput output)
            {
                Guard.IsNotNull(context, nameof(context));
                Guard.IsNotNull(output, nameof(output));

                var select2builder = new TagBuilder(_tagName);
                var spanBuilder = new TagBuilder("span");

                output.TagName = "div";
                output.TagMode = TagMode.StartTagAndEndTag;

                // e.g. v-on:change="DoSomeClientSideLogic". If we don't do this the vue directive will be attached to the div. 
                if (output.Attributes.Any()) 
                {
                    foreach (var attribute in output.Attributes)
                    {
                        if (attribute.Name.StartsWith("v-"))
                            select2builder.MergeAttribute($"{attribute.Name}", attribute.Value.ToString());
                    }
                }

                if (For is null)
                    throw new ArgumentNullException(nameof(For));

                if (string.IsNullOrWhiteSpace(Name))
                    Name = For.Name;

                if (!string.IsNullOrWhiteSpace(Selections))
                    select2builder.MergeAttribute(":options", Selections);

                if (Disabled)
                    select2builder.MergeAttribute(":disabled", Disabled.ToString().ToLower());

                var requiredAttribute = For.Metadata.GetCustomAttributes(typeof(RequiredAttribute)).FirstOrDefault();
                if (requiredAttribute != null)
                {
                    RequiredMessage = (requiredAttribute as RequiredAttribute)?.ErrorMessage;
                    Required = true;
                }

                if (Required)
                {
                    if (string.IsNullOrWhiteSpace(RequiredMessage))
                        RequiredMessage = _defaultRequiredMessage;

                    select2builder.MergeAttribute(":required", Required.ToString().ToLower());
                    select2builder.MergeAttribute(":required-message", $"'{RequiredMessage}'");

                    spanBuilder.AddCssClass("field-validation-valid");
                    spanBuilder.MergeAttribute("data-valmsg-for", Name);
                    spanBuilder.MergeAttribute("data-valmsg-replace", "true");
                }

                select2builder.GenerateId($"{Name}_{_tagName}", "i");
                select2builder.MergeAttribute("v-model", Name);
                select2builder.MergeAttribute(":multiple", Multiple.ToString().ToLower());
                select2builder.MergeAttribute(":name", $"'{Name}'");

                output.Content.AppendHtml(select2builder);
                if (Required)
                    output.Content.AppendHtml(spanBuilder);
            }
        }
    }
}
