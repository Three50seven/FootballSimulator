using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Concurrent;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Common.AspNetCore.Mvc.TagHelpers
{
    [HtmlTargetElement("svg", Attributes = "name", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class SvgTagHelper : TagHelper
    {
        public static string ViewsDirectory = "~/Views/Shared/SVGs/";

        private readonly IHtmlHelper _htmlHelper;
        private readonly HtmlEncoder _htmlEncoder;
        private static readonly ConcurrentDictionary<string, string> _cachedSvgs = new();

        public SvgTagHelper(IHtmlHelper htmlHelper, HtmlEncoder htmlEncoder)
        {
            _htmlHelper = htmlHelper;
            _htmlEncoder = htmlEncoder;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("name")]
        public string Name { get; set; }

        [HtmlAttributeName("cache")]
        public bool Cache { get; set; } = true;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException(nameof(Name));

            output.TagName = null;
            output.TagMode = TagMode.StartTagAndEndTag;
            string cacheKey = Name.ToLower();

            if (Cache && _cachedSvgs.TryGetValue(cacheKey, out string value))
            {
                output.PostContent.SetHtmlContent(value);
            }
            else
            {
                (_htmlHelper as IViewContextAware).Contextualize(ViewContext);

                var view = await _htmlHelper.PartialAsync($"{ViewsDirectory}{Name}.cshtml");
                string html = view.ToString(_htmlEncoder);

                if (Cache)
                    _cachedSvgs.TryAdd(cacheKey, html);

                output.PostContent.SetHtmlContent(html);
            }
        }
    }
}
