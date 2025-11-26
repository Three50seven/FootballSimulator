using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Common.Core.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.AspNetCore.Mvc.TagHelpers
{
    [HtmlTargetElement("validation-summary")]
    public class ValidationSummaryTagHelper : TagHelper
    {
        public static string DefaultIntroMessage = string.Empty;

        [HtmlAttributeName("only-broken-rules")]
        [Obsolete("Please use IncludeAllRules ('include-all-rules') instead.")]
        public bool OnlyBrokenRules { get; set; } = true;

        [HtmlAttributeName("include-all-rules")]
        public bool IncludeAllRules { get; set; } = false;

        [HtmlAttributeName("intro-message")]
        public string IntroMessage { get; set; } = DefaultIntroMessage;

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";

            output.Attributes.AddCssClass("validation-errors-container");

            if (IncludeAllRules)
                output.Attributes.Add("data-valmsg-summary", "true");

            if (ViewContext.ModelState.IsValid)
            {
                output.Attributes.AddCssClass("validation-summary-valid");
                output.Attributes.AddIfAbsent("style", "display:none;");

                output.Content.AppendHtml("<ul><li style='display:none'></li></ul>"); // for front-end validation container
            }
            else
            {
                output.Attributes.AddCssClass("validation-summary-errors");
                output.Attributes.AddCssClass("field-validation-error");
                output.Attributes.AddCssClass("error-highlight");

                var allErrors = ViewContext.ModelState.GetAllErrors();
                var brokenRuleOnlyErrors = ViewContext.ModelState.GetErrorsByKey(BrokenRulesList.ModelStateKey);

                // if custom broken rules exist, set element to not allow hiding when form validation is cleared
                if (brokenRuleOnlyErrors.Any())
                    output.Attributes.AddCssClass("validation-no-hide");

                if (!string.IsNullOrWhiteSpace(IntroMessage))
                {
                    var p = new TagBuilder("p");
                    p.AddCssClass("validation-errors-default-message");
                    p.AddCssClass("error-highlight-msg");

                    if (brokenRuleOnlyErrors.Any())
                        p.AddCssClass("validation-no-hide");

                    p.InnerHtml.Append(IntroMessage);
                    output.Content.AppendHtml(p);
                }   

                // show all errors if set to display all, otherwise just show the custom broken rules errors
                if (allErrors.Any() && IncludeAllRules)
                    output.Content.AppendHtml(BuildErrorList(allErrors));
                else if (brokenRuleOnlyErrors.Any())
                    output.Content.AppendHtml(BuildErrorList(brokenRuleOnlyErrors));
            }
        }

        private static TagBuilder BuildErrorList(IEnumerable<string> errors)
        {
            var ul = new TagBuilder("ul");

            foreach (var error in errors)
            {
                var li = new TagBuilder("li");
                li.AddCssClass("error-highlight-msg");
                li.InnerHtml.Append(error);

                ul.InnerHtml.AppendHtml(li);
            }

            return ul;
        }
    }
}
