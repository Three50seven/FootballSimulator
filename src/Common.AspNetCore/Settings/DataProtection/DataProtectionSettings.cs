namespace Common.AspNetCore
{
    public class DataProtectionSettings
    {
        public string ApplicationName { get; set; } = "webapp";
        public string KeyStorageDirectory { get; set; } = "/keys";
    }
}
