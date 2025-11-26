namespace Common.AspNetCore
{
    public class UniqueRequestIdOptions
    {
        public const string DefaultKey = HttpContextConstants.UniqueRequestIdKey;

        public string Key { get; set; } = DefaultKey;
        public bool Override { get; set; } = false;
    }
}
