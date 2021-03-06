﻿
namespace HealthCloudClient
{
    using System;

    /// <summary>
    /// A Run segment
    /// </summary>
    public class RunSegment
    {
        /// <value>The value of distance data during the segment</value>
        public DistanceSummary DistanceSummary { get; set; }
        /// <value>The length of time the user was paused during the segment</value>
        public TimeSpan? PausedDuration { get; set; }
        /// <value>The mapping of the amount of time spent in a given heart rate zone during the segment</value>
        public HeartRateZones HeartRateZones { get; set; }
        /// <value>The split distance used for the segment</value>
        public int? SplitDistance { get; set; }
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
