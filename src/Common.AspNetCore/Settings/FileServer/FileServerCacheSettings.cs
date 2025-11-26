namespace Common.AspNetCore
{
    public class FileServerCacheSettings
    {
        public bool Enabled { get; set; } = false;
        public int Duration { get; set; } = StaticFilesExtensions.DefaultCacheControlAge;
    }
}
