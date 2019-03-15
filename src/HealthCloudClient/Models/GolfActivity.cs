
namespace HealthCloudClient
{
    using System;

    /// <summary>
    /// A Golf activity
    /// </summary>
    public class GolfActivity
    {
        /// <value>
        /// The type of this activity
        /// ['Unknown', 'Custom', 'CustomExercise', 'CustomComposite', 'Run', 'Sleep', 'FreePlay', 'GuidedWorkout', 'Bike', 'Golf', 'RegularExercise', 'Hike']
        /// </value>
        public string ActivityType { get; set; }
        /// <value>The segments associated with this activity</value>
        public GolfHoleSegment[] ActivitySegments { get; set; }
        /// <value>The total number of shots a user took during the activity</value>
        public int? TotalScore { get; set; }
        /// <value>The total number of steps a user took during the activity</value>
        public int? TotalStepCount { get; set; }
        /// <value>The total distance a user walked during the activity</value>
        public int? TotalDistanceWalked { get; set; }
        /// <value>The number of holes played where the user scored par or better during the activity</value>
        public int? ParOrBetterCount { get; set; }
        /// <value>The distance of the longest drive hit by the user during the activity</value>
        public int? LongestDriveDistance { get; set; }
        /// <value>The distance of the longest stroke hit by the user during the activity</value>
        public int? LongestStrokeDistance { get; set; }
        /// <value>The list of child activities</value>
        public Activity[] ChildActivities { get; set; }
        /// <value>The unique identifier of the activity (unique by user)</value>
        public string Id { get; set; }
        /// <value>The unique identifier of the user who completed the activity</value>
        public string UserId { get; set; }
        /// <value>The identifier of the device which generated the activity</value>
        public string DeviceId { get; set; }
        /// <value>The start time of the activity</value>
        public DateTime? StartTime { get; set; }
        /// <value>The end time of the activity</value>
        public DateTime? EndTime { get; set; }
        /// <value>
        /// The mapping of an event to a logical date. For most events, other than sleep,
        /// the default assignment is based on the event's start time. This is subject to change in the future.
        /// For example, if a sleep activity starts before 5 AM, the DayId is the previous day.
        /// </value>
        public string DayId { get; set; }
        /// <value>The time the activity was created</value>
        public DateTime? CreatedTime { get; set; }
        /// <value>The app that created the activity</value>
        public string CreatedBy { get; set; }
        /// <value>The name of the activity</value>
        public string Name { get; set; }
        /// <value>The duration of the activity</value>
        public TimeSpan? Duration { get; set; }
        /// <value>The summaries associated with this activity</value>
        public Summary[] MinuteSummaries { get; set; }
        /// <value>The value of calories burned during the activity</value>
        public CaloriesBurnedSummary CaloriesBurnedSummary { get; set; }
        /// <value>The heart rate value for the activity</value>
        public HeartRateSummary HeartRateSummary { get; set; }
        /// <value>The UV exposure as time in the sun</value>
        public string UvExposure { get; set; }
    }
}
