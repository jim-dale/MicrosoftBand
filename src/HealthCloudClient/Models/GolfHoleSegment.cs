
namespace HealthCloudClient
{
    using System;

    /// <summary>
    /// A Golf Hole segment
    /// </summary>
    public class GolfHoleSegment
    {
        /// <value>The par for the hole on the golf course</value>
        public int? HolePar { get; set; }
        /// <value>The hole number on the golf course</value>
        public int? HoleNumber { get; set; }
        /// <value>The steps taken by the user for the hole</value>
        public int? StepCount { get; set; }
        /// <value>The distance walked by the user for the hole</value>
        public int? DistanceWalked { get; set; }
        /// <value>The unique identifier of the segment</value>
        public long? SegmentId { get; set; }
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
        /// <value>
        /// The segment type
        /// ['Unknown', 'Run', 'FreePlay', 'Doze', 'Sleep', 'Snooze', 'Awake', 'GuidedWorkout', 'Bike', 'Pause',
        /// 'Resume', 'DistanceBasedInterval', 'TimeBasedInterval', 'GolfHole', 'GolfShot', 'NotWorn', 'Hike']
        /// </value>
        public string SegmentType { get; set; }
    }
}
