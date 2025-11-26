using Microsoft.AspNetCore.Razor.TagHelpers;
using Common.Core.Validation;

namespace Common.AspNetCore
{
    public static class TagHelperOutputExtensions
    {
        /// <summary>
        /// Updates output to be a "script" tag based on the external reference <paramref name="externalReference"/>.
        /// Will append fallback script logic if <see cref="ExternalContentReference.FallbackOperation"/> and <see cref="ExternalContentReference.FallbackPath"/> are supplied.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="externalReference"></param>
        /// <returns></returns>
        public static TagHelperOutput ApplyExternalReference(this TagHelperOutput output, ExternalContentReference externalReference)
        {
            Guard.IsNotNull(output, nameof(output));
            Guard.IsNotNull(externalReference, nameof(externalReference));

            output.TagName = "script";

            output.Attributes.Add("src", externalReference.Url);
            if (!string.IsNullOrWhiteSpace(externalReference.FallbackPath) && !string.IsNullOrWhiteSpace(externalReference.FallbackOperation))
                output.PostElement.AppendHtml($"<script type=\"text/javascript\">function loadFallback() {{ let xhrReq = new XMLHttpRequest(); xhrReq.open('GET', '{externalReference.FallbackPath}', false); xhrReq.send(''); eval(xhrReq.responseText); }}  {externalReference.FallbackOperation} || loadFallback(); </script>");

            return output;
        }

        /// <summary>
        /// Append "script" tags to the output via <see cref="TagHelperContent.AppendHtml(string)"/>.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="scripts">List of source script Urls for the "src" script tag attribute.</param>
        /// <returns></returns>
        public static TagHelperOutput AppendScriptTags(this TagHelperOutput output, IEnumerable<string> scripts)
        {
            Guard.IsNotNull(output, nameof(output));

            foreach (var script in scripts)
            {
                output.PostElement.AppendHtml($"<script src='{script}'></script>");
            }

            return output;
        }
    }
}
