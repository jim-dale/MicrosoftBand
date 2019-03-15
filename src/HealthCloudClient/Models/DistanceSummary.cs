
namespace HealthCloudClient
{
    /// <summary>
    /// Summary of the distance data gathered during an exercise
    /// </summary>
    public class DistanceSummary
    {
        /// <value>The length of the time bucket for which the value is calculated = ['Unknown', 'Activity', 'Minute', 'Hourly', 'Daily', 'Segment']</value>
        public string Period { get; set; }
        /// <value>
        /// The total distance during the period.
        /// If this is a time-based value, e.g.hourly or daily, then this is the total distance of the period.
        /// If this is an activity split value, e.g.splits of a run, then this is the split distance, e.g. 1 mile, 1 kilometer.
        /// For the last split of the activity, this value may be less than the full split distance.
        /// </value>
        public int? TotalDistance { get; set; }
        /// <value>The total distance covered on foot during the period</value>
        public int? TotalDistanceOnFoot { get; set; }
        /// <value>The absolute distance including any paused time distance during the period</value>
        public int? ActualDistance { get; set; }
        /// <value>The cumulative elevation gain accrued during the period in cm</value>
        public int? ElevationGain { get; set; }
        /// <value>The cumulative elevation loss accrued during this period in cm</value>
        public int? ElevationLoss { get; set; }
        /// <value>The maximum elevation during this period in cm</value>
        public int? MaxElevation { get; set; }
        /// <value>The minimum elevation during this period in cm</value>
        public int? MinElevation { get; set; }
        /// <value>The distance in cm between recorded GPS points</value>
        public int? WaypointDistance { get; set; }
        /// <value>The average speed during the period</value>
        public int? Speed { get; set; }
        /// <value>The average pace during the period</value>
        public int? Pace { get; set; }
        /// <value>The total distance to the end of this period divided by total time to the end of this period</value>
        public int? OverallPace { get; set; }
    }
}
