using System.Collections.Generic;

namespace Common.AspNetCore
{
    public class UrlDecodingSettings
    {
        public IReadOnlyDictionary<string, string> CustomCharacterMatches { get; set; } = new Dictionary<string, string>();
    }
}
