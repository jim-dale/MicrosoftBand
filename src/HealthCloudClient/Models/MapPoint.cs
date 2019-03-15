
namespace HealthCloudClient
{
    /// <summary>
    /// A map point for an exercise
    /// </summary>
    public class MapPoint
    {
        /// <value>The number of seconds that have elapsed since mapping began, typically the start of a run or other activity</value>
        public int? SecondsSinceStart { get; set; }
        /// <value>The type of map point = ['Unknown', 'Start', 'End', 'Split', 'Waypoint']</value>
        public string MapPointType { get; set; }
        /// <value>The absolute ordering of this point relative to the others in its set, starting from 0</value>
        public int? Ordinal { get; set; }
        /// <value>The distance not including distance traveled while paused,it is the distance that splits are based off of, since splits ignore paused time</value>
        public int? ActualDistance { get; set; }
        /// <value>The total distance from the start point to this map point</value>
        public int? TotalDistance { get; set; }
        /// <value>The heart rate at the time of this map point</value>
        public int? HeartRate { get; set; }
        /// <value>The pace</value>
        public int? Pace { get; set; }
        /// <value>
        /// A number between 0 and 100 that denotes the pace/speed between the slowest
        /// and fastest instantaneous pace for the overall route.Slowest segment in the route (highest pace, lowest speed)
        /// is 0 and fastest segment (lowest pace, highest speed) is 100. Only makes sense in the context of the set of all map points.
        /// </value>
        public int? ScaledPace { get; set; }
        /// <value>The speed</value>
        public int? Speed { get; set; }
        /// <value>The GPS location for this map point</value>
        public GpsPoint Location { get; set; }
        /// <value>A value indicating whether or not this map point occurred during paused time</value>
        public bool? IsPaused { get; set; }
        /// <value>A value indicating whether or not this map point is the first one since the activity resumed</value>
        public bool? IsResume { get; set; }
    }
}
