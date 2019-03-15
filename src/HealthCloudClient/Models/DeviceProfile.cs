
namespace HealthCloudClient
{
    using System;

    /// <summary>
    /// Data about a particular device used to capture exercise data
    /// </summary>
    public class DeviceProfile
    {
        /// <value>The unique identifier of the device</value>
        public string Id { get; set; }
        /// <value>The name the user has given the device. (Not available in the Developer Preview)</value>
        public string DisplayName { get; set; }
        /// <value>The date the device was last synced with the service. (Not available in the Developer Preview)</value>
        public DateTime? LastSuccessfulSync { get; set; }
        /// <value>The device family = ['Unknown', 'Band', 'Windows', 'Android', 'IOS']</value>
        public string DeviceFamily { get; set; }
        /// <value>The device version</value>
        public string HardwareVersion { get; set; }
        /// <value>The device's software version</value>
        public string SoftwareVersion { get; set; }
        /// <value>The name of the model of the device</value>
        public string ModelName { get; set; }
        /// <value>The name of the manufacturer of the device</value>
        public string Manufacturer { get; set; }
        /// <value>The current status of the device = ['Unknown', 'Active', 'Inactive']</value>
        public string DeviceStatus { get; set; }
        /// <value>The date the device was first registered(Not available in the Developer Preview)</value>
        public DateTime? CreatedDate { get; set; }
    }
}
