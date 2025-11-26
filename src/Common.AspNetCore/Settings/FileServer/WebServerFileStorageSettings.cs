using System.Collections.Generic;
using System.Linq;

namespace Common.AspNetCore
{
    public class WebFileServersConfigurationSettings
    {
        public IEnumerable<WebFileServerSettings> Servers { get; set; }

        public IEnumerable<WebFileServerOptions> FileServerOptions => Servers?.Select(s => s.ToWebFileServerOptions());
    }
}
