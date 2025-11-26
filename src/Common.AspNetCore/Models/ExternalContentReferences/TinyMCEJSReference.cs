namespace Common.AspNetCore
{
    public class TinyMCEJSReference : ExternalContentReference
    {
        public TinyMCEJSReference(string version)
            : base($"https://cdnjs.cloudflare.com/ajax/libs/tinymce/{version}/tinymce.min.js",
                  $"{JSFilePublishedDirectoryPath}lib/tinymce/tinymce.min.js")
        {
        }

        public TinyMCEJSReference(string url, string fallbackPath) 
            : base(url, fallbackPath)
        {
        }

        public override string FallbackOperation => "window.tinymce";
    }
}
