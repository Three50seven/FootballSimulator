namespace Common.AspNetCore
{
    public class JQueryUIReference : ExternalContentReference
    {
        public JQueryUIReference(string version)
            : this($"https://ajax.googleapis.com/ajax/libs/jqueryui/{version}/jquery-ui.min.js",
                  $"{JSFilePublishedDirectoryPath}lib/jquery-ui.min.js")
        { }

        public JQueryUIReference(string url, string fallbackPath) : base(url, fallbackPath)
        {

        }

        public override string FallbackOperation => "window.jQuery.ui";
    }
}
