using System.Collections.Generic;
using System.Net;

namespace Common.AspNetCore
{
    public class AjaxRedirectHeaderOptions
    {
        public string HeaderName { get; set; } = "REDIRECT_LOCATION";
        public int StatusCodeForResponse { get; set; } = (int)HttpStatusCode.OK;
        public IEnumerable<string> Locations { get; set; } = new List<string>();
    }
}
