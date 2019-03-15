
namespace HealthCloudClient
{
    /// <summary>
    /// The heart rate summary for the exercise
    /// </summary>
    public class HeartRateSummary
    {
        /// <value>The length of the time bucket for which the value is calculated ['Unknown', 'Activity', 'Minute', 'Hourly', 'Daily', 'Segment']</value>
        public string Period { get; set; }
        /// <value>The average heart rate achieved during the period</value>
        public int? AverageHeartRate { get; set; }
        /// <value>The peak heart rate achieved during the period</value>
        public int? PeakHeartRate { get; set; }
        /// <value>The lowest heart rate achieved during the period</value>
        public int? LowestHeartRate { get; set; }
    }
}
