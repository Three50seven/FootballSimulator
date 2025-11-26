namespace Common.AspNetCore
{
    public class JQueryReference : ExternalContentReference
    {
        public JQueryReference(string version)
            : this($"https://ajax.googleapis.com/ajax/libs/jquery/{version}/jquery.min.js",
                  $"{JSFilePublishedDirectoryPath}lib/jquery.min.js")
        { }

        public JQueryReference(string url, string fallbackPath) : base(url, fallbackPath)
        {

        }

        public override string FallbackOperation => "window.jQuery";
    }
}
