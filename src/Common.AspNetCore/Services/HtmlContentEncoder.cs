using Common.Core;
using System.Text.Encodings.Web;

namespace Common.AspNetCore.Services
{
    public class HtmlContentEncoder : IContentEncoder
    {
        private readonly HtmlEncoder _htmlEncoder;

        public HtmlContentEncoder(HtmlEncoder htmlEncoder)
        {
            _htmlEncoder = htmlEncoder;
        }

        public virtual string Encode(string value)
        {
            return _htmlEncoder.Encode(value).ReplaceNewLinesForHtml();
        }
    }
}
