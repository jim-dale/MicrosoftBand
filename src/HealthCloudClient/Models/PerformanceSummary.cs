
namespace HealthCloudClient
{
    /// <summary>
    /// Performance summary data
    /// </summary>
    public class PerformanceSummary
    {
        /// <value>The heart rate when the user finished the exercise</value>
        public int? FinishHeartRate { get; set; }
        /// <value>The heart rate one minute after the user finished the exercise</value>
        public int? RecoveryHeartRateAt1Minute { get; set; }
        /// <value>The heart rate two minutes after the user finished the exercise</value>
        public int? RecoveryHeartRateAt2Minutes { get; set; }
        /// <value>The breakdown of the heart rate zones during the exercise</value>
        public HeartRateZones HeartRateZones { get; set; }
    }
}
