namespace Common.AspNetCore
{
    public abstract class ExternalContentReference
    {
        public static string JSFilePublishedDirectoryPath = "/assets/js/";
        public static string JSFileSourceDirectoryPath = "/scripts/";

        protected ExternalContentReference(string url, string fallbackPath)
        {
            Url = url;
            FallbackPath = fallbackPath;
        }

        public string Url { get; private set; }
        public string FallbackPath { get; private set; }
        public abstract string FallbackOperation { get; }
    }
}
