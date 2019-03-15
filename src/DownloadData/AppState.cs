
namespace HealthSdkPrototype
{
    using System;
    using HealthCloudClient;

    /// <summary>
    /// Application state cached in JSON file
    /// </summary>
    public class AppState
    {
        public DateTime AccountCreatedTimeUtc { get; set; }
        public DateTime AccountLastUpdateTimeUtc { get; set; }
        public DateTime LastDailySummarySyncTimeUtc { get; set; }
        public DateTime LastHourlySummarySyncTimeUtc { get; set; }
        public DateTime LastDailyActivitiesSyncTimeUtc { get; set; }
        public DateTime AccessTokenExpirationTimeUtc { get; set; }
        public AccessToken AccessToken { get; set; }
    }
}
