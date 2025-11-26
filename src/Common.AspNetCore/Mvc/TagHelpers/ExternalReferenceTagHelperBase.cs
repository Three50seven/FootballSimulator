using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;

namespace Common.AspNetCore.Mvc.TagHelpers
{
    public abstract class ExternalReferenceTagHelperBase : TagHelper
    {
        protected ExternalReferenceTagHelperBase(IBundledFileResolver bundledFileResolver)
        {
            BundledFileResolver = bundledFileResolver;
        }

        protected IBundledFileResolver BundledFileResolver { get; }
        protected abstract ExternalContentReference ExternalReference { get; }
        protected abstract IEnumerable<string> AssetReferences { get; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.ApplyExternalReference(ExternalReference);
            var scripts = BundledFileResolver.Resolve(AssetReferences?.ToArray());
            output.AppendScriptTags(scripts);
        }
    }
}
