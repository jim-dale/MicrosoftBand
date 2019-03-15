
namespace HealthCloudClient
{
    /// <summary>
    /// The GPS location for a map point
    /// </summary>
    public class GpsPoint
    {
        /// <value>The speed over ground for the GPS point in m/s* 100</value>
        public int? SpeedOverGround { get; set; }
        /// <value>The latitude for the GPS point in degrees* 10^7 (+ = North)</value>
        public int? Latitude { get; set; }
        /// <value>The longitude for the GPS point in degrees* 10^7 (+ = East)</value>
        public int? Longitude { get; set; }
        /// <value>The elevation from mean sea level in m* 100</value>
        public int? ElevationFromMeanSeaLevel { get; set; }
        /// <value>The estimated horizontal error in m* 100</value>
        public int? EstimatedHorizontalError { get; set; }
        /// <value>The estimated vertical error in m* 100</value>
        public int? EstimatedVerticalError { get; set; }
    }
}
