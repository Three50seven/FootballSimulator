using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Common.Core.Domain;
using System.Threading.Tasks;
using System.Web;

namespace Common.AspNetCore.Mvc.TagHelpers
{
    [HtmlTargetElement("img", Attributes = "src, resize-w, resize-h")]
    public class ImageResizeTagHelper : TagHelper
    {
        [HtmlAttributeName("resize-w")]
        public int ResizeWidth { get; set; }

        [HtmlAttributeName("resize-h")]
        public int ResizeHeight { get; set; }

        [HtmlAttributeName("as-percentage")]
        public bool? AsPercentage { get; set; }

        [HtmlAttributeName("format")]
        public ImageResizeFormatOption Format { get; set; } = ImageResizeFormatOption.Pad;

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var resizeSettings = new ImageResizeSettings(new ImageDimension(ResizeWidth, ResizeHeight, AsPercentage ?? false), Format);

            var src = (HtmlString)output.Attributes["src"].Value;
            var resizeQuery = resizeSettings.ToQueryString();

            output.Attributes.SetAttribute("src", string.Concat(src.Value, src.Value.Contains('?') ? "&" : "?", resizeQuery));

            return Task.CompletedTask;
        }
    }
}
