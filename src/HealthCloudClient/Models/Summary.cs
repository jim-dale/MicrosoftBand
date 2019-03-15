
namespace HealthCloudClient
{
    using System;

    /// <summary>
    /// Summary record
    /// </summary>
    public class Summary
    {
        /// <value>The unique identifier of the user</value>
        public string UserId { get; set; }
        /// <value>The start time of the period</value>
        public DateTime? StartTime { get; set; }
        /// <value>The end time of the period</value>
        public DateTime? EndTime { get; set; }
        /// <value>The parent day of the period</value>
        public DateTime? ParentDay { get; set; }
        /// <value>True if the user transitioned time zones during this period, else false</value>
        public bool? IsTransitDay { get; set; }
        /// <value>The length of the time bucket for which the value is calculated = ['Unknown', 'Activity', 'Minute', 'Hourly', 'Daily', 'Segment']</value>
        public string Period { get; set; }
        /// <value>The duration of the period</value>
        public TimeSpan? Duration { get; set; }
        /// <value>The total number of steps taken during the period</value>
        public int? StepsTaken { get; set; }
        /// <value>The total number of floors climbed during the period</value>
        public int? FloorsClimbed { get; set; }
        /// <value>The value of the calories burned during the period</value>
        public CaloriesBurnedSummary CaloriesBurnedSummary { get; set; }
        /// <value>The value of heart rate data during the period</value>
        public HeartRateSummary HeartRateSummary { get; set; }
        /// <value>The value of the distance data during the period</value>
        public DistanceSummary DistanceSummary { get; set; }
        /// <value>The number of active hours during the period.An active hour is an hour with more than 600 steps</value>
        public int? ActiveHours { get; set; }
        /// <value>The UV exposure as time in the sun</value>
        public string UvExposure { get; set; }
        /// <value>The number of active seconds during the period</value>
        public int? ActiveSeconds { get; set; }
    }
}
