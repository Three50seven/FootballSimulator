using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Common.Core.Validation;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;

namespace Common.AspNetCore.Mvc.TagHelpers
{
    [HtmlTargetElement("render-vue-template", Attributes = "name", TagStructure = TagStructure.WithoutEndTag)]
    public class VueTemplateRenderTagHelper : PartialTagHelper
    {
        private string _originalName;

        public VueTemplateRenderTagHelper(ICompositeViewEngine viewEngine, IViewBufferScope viewBufferScope)
            : base(viewEngine, viewBufferScope)
        {

        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            Guard.IsNotNull(context, nameof(context));
            Guard.IsNotNull(output, nameof(output));

            bool viewFound = true;

            try
            {
                FormatName();
                await base.ProcessAsync(context, output);
            }
            catch (InvalidOperationException)
            {
                viewFound = false;
            }

            if (!viewFound)
            {
                Name = _originalName;
                await base.ProcessAsync(context, output);
            }
        }

        private void FormatName()
        {
            if (string.IsNullOrWhiteSpace(_originalName))
                _originalName = Name;

            var name = CheckRemoveExtension(Name);
            if (!name.Contains("VueTemplate"))
                name = string.Concat(Path.GetFileNameWithoutExtension(name), "VueTemplate");

            Name = name;
        }

        private static string CheckRemoveExtension(string path, string ext = "cshtml")
        {
            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(ext))
                return path;

            if (!path.EndsWith(ext))
                return path;

            if (!ext.StartsWith('.'))
                ext = $".{ext}";

            return path.Replace(ext, "");
        }
    }
}
