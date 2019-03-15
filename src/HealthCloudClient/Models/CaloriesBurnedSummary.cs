
namespace HealthCloudClient
{
    /// <summary>
    /// Summary of the calories burned during an activity or segment
    /// </summary>
    public class CaloriesBurnedSummary
    {
        /// <value>The length of the time bucket for which the value is calculated = ['Unknown', 'Activity', 'Minute', 'Hourly', 'Daily', 'Segment']</value>
        public string Period { get; set; }
        /// <value>The total calories burned during the period</value>
        public int? TotalCalories { get; set; }
    }
}
