using System.Collections.Generic;

namespace Common.AspNetCore
{
    public class WebFileServerSettings
    {
        public string PhysicalPath { get; set; }

        public string RequestPath { get; set; }

        public bool PhysicalPathIsRelative { get; set; } = false;

        public bool AllowDirectoryBrowsing { get; set; } = false;

        public bool AllowUnknownFileTypes { get; set; } = false;
        public bool CreateDirectory { get; set; } = true;

        public IDictionary<string, string> ContentTypes { get; set; } = new Dictionary<string, string>();

        public FileServerCacheSettings Cache { get; set; } = new FileServerCacheSettings();
    }
}
