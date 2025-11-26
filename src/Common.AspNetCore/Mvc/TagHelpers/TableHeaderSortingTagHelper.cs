using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Common.Core.Domain;
using Common.Core.Validation;
using System.Threading.Tasks;

namespace Common.AspNetCore.Mvc.TagHelpers
{
    [HtmlTargetElement("th", Attributes=_sortInfoAttributeName)]
    [HtmlTargetElement("th", Attributes=_sortColumnAttributeName)]
    public class TableHeaderSortingTagHelper : TagHelper
    {
        private const string _sortColumnAttributeName = "sort-column";
        private const string _sortInfoAttributeName = "sort-info";
        private const string _sortableCssClass = "sortable";
        private const string _dataSortAttributeName = "data-sort-column";
        private static readonly bool _useAnchorTag = true;

        [HtmlAttributeName(_sortInfoAttributeName)]
        public SortCriteria SortingInfo { get; set; }

        [HtmlAttributeName(_sortColumnAttributeName)]
        public string SortColumn { get; set; }

        [HtmlAttributeNotBound]
        protected bool IsCurrentSort => SortingInfo.SortBy.Equals(SortColumn);

        [HtmlAttributeNotBound]
        protected string SortDirectionClass => $"sorted-{SortingInfo?.DirectionAbbr}";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (SortingInfo == null || string.IsNullOrEmpty(SortColumn))
                return;

            Guard.IsNotNull(context, nameof(context));
            Guard.IsNotNull(output, nameof(output));

            if (_useAnchorTag)
            {
                var anchorTagBuilder = new TagBuilder("a");
                anchorTagBuilder.AddCssClass(_sortableCssClass);
                anchorTagBuilder.Attributes.Add(_dataSortAttributeName, SortColumn);
                anchorTagBuilder.Attributes.Add("href", "#");
                anchorTagBuilder.TagRenderMode = TagRenderMode.Normal;

                if (IsCurrentSort)
                    anchorTagBuilder.AddCssClass(SortDirectionClass);

                var content = await output.GetChildContentAsync();
                anchorTagBuilder.InnerHtml.Append(content.GetContent());
                
                output.Content.AppendHtml(anchorTagBuilder);
            }
            else
            {
                output.Attributes.AddCssClass(_sortableCssClass);
                output.Attributes.Add(_dataSortAttributeName, SortColumn);

                if (IsCurrentSort)
                    output.Attributes.AddCssClass(SortDirectionClass);
            }
        }
    }
}
