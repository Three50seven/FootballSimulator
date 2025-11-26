namespace Common.AspNetCore
{
    public class CustomCharactersUrlDecoder : IUrlDecoder
    {
        private readonly UrlDecodingSettings _decodingSettings;

        public CustomCharactersUrlDecoder(UrlDecodingSettings decodingSettings)
        {
            _decodingSettings = decodingSettings;
        }

        public string Decode(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return url;

            foreach (var match in _decodingSettings.CustomCharacterMatches)
            {
                url = url.Replace(match.Key, match.Value);
            }

            return url.Trim();
        }
    }
}
