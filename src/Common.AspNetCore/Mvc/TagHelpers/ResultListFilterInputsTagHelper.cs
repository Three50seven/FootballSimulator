using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Common.AspNetCore.Mvc.TagHelpers
{
    [HtmlTargetElement("result-list-filter-inputs", Attributes = "for", TagStructure = TagStructure.WithoutEndTag)]
    public class ResultListFilterInputsTagHelper : TagHelper
    {
        private readonly IHtmlHelper _htmlHelper;

        public ResultListFilterInputsTagHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("for")]
        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ((IViewContextAware)_htmlHelper).Contextualize(ViewContext);

            var result = _htmlHelper.Editor(For.Name, "ResultListFilterHiddenInputs");
            output.Content.SetHtmlContent(result);

            output.TagName = null;
        }
    }
}
