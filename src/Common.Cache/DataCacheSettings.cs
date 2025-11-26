namespace Common.Cache
{
    /// <summary>
    /// Settings regarding storing data in cache.
    /// </summary>
    public class DataCacheSettings
    {
        /// <summary>
        /// Profiles of durations in which data should be stored in cache.
        /// </summary>
        public IDictionary<string, TimeSpan> Profiles { get; set; } = new Dictionary<string, TimeSpan>();

        /// <summary>
        /// Default time for data cache values before they expire. If not set, <see cref="Common.Core.CacheConstants.DEFAULT_RETENTION_TIME"/> is used.
        /// </summary>
        public double DefaultRetention { get; set; }

        /// <summary>
        /// Whether caching should be enabled. Defaults to true.
        /// </summary>
        public bool Enabled { get; set; } = true;
    }
}
