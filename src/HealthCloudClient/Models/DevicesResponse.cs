
namespace HealthCloudClient
{

    /// <summary>
    /// Container for a list of DeviceProfile records returned by the REST API
    /// </summary>
    public class DevicesResponse
    {
        /// <value>The collection of device profiles</value>
        public DeviceProfile[] DeviceProfiles { get; set; }
        /// <value>The number of items returned</value>
        public int? ItemCount { get; set; }
    }
}
