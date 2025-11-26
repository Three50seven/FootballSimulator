using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace Common.AspNetCore.Mvc
{
    [HtmlTargetElement("multi-line-text", Attributes = "text", TagStructure = TagStructure.WithoutEndTag)]
    public class MultiLineTextTagHelper : TagHelper
    {
        [HtmlAttributeName("text")]
        public string Text { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;

            if (string.IsNullOrWhiteSpace(Text))
                return;

            var paragraphs = Text.Split(Environment.NewLine);

            foreach (var paragraph in paragraphs)
            {
                var paragraphTagBuilder = new TagBuilder("p")
                {
                    TagRenderMode = TagRenderMode.Normal
                };
                paragraphTagBuilder.InnerHtml.Append(paragraph);
                output.Content.AppendHtml(paragraphTagBuilder);
            }
        }
    }
}
