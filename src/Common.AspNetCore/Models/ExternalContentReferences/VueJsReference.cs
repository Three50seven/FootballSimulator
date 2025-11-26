namespace Common.AspNetCore
{
    public class VueJSReference : ExternalContentReference
    {
        public VueJSReference(string version, bool debug = false)
            : base($"https://cdn.jsdelivr.net/npm/vue@{version}/dist/{(debug ? "vue.js" : "vue.min.js")}",
                  debug ? $"{JSFileSourceDirectoryPath}lib/vue.js" : $"{JSFilePublishedDirectoryPath}lib/vue.min.js")
        {
        }

        public VueJSReference(string url, string fallbackPath) 
            : base(url, fallbackPath)
        {

        }


        public override string FallbackOperation => "window.Vue";
    }
}
