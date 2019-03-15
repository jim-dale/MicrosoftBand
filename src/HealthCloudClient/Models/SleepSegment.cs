
namespace HealthCloudClient
{
    using System;

    /// <summary>
    /// A Sleep segment
    /// </summary>
    public class SleepSegment
    {
        /// <value>The length of time in minutes the user was asleep during the segment</value>
        public int? SleepTime { get; set; }
        /// <value>The mapping of the sleep segment to a logical date.This is the same as the DayId for the sleep activity</value>
        public string DayId { get; set; }
        /// <value>The sleep state = ['Unknown', 'UndifferentiatedSleep', 'RestlessSleep', 'RestfulSleep']</value>
        public string SleepType { get; set; }
        /// <value>The unique identifier of the segment</value>
        public int? SegmentId { get; set; }
        /// <value>The start time of the segment</value>
        public DateTime? StartTime { get; set; }
        /// <value>The end time of the segment</value>
        public DateTime? EndTime { get; set; }
        /// <value>The duration of the segment</value>
        public TimeSpan? Duration { get; set; }
        /// <value>The heart rate value for the segment</value>
        public HeartRateSummary HeartRateSummary { get; set; }
        /// <value>The value of calories burned during the segment</value>
        public CaloriesBurnedSummary CaloriesBurnedSummary { get; set; }
        /// <value>The segment type
        /// ['Unknown', 'Run', 'FreePlay', 'Doze', 'Sleep', 'Snooze', 'Awake', 'GuidedWorkout', 'Bike', 'Pause',
        /// 'Resume', 'DistanceBasedInterval', 'TimeBasedInterval', 'GolfHole', /// 'GolfShot', 'NotWorn', 'Hike']
        /// </value>
        public string SegmentType { get; set; }
    }
}
