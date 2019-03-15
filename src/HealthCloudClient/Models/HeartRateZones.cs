
namespace HealthCloudClient
{
    using System;

    /// <summary>
    /// The breakdown of the heart rate zones during an exercise
    /// </summary>
    public class HeartRateZones
    {
        /// <value>The number of minutes where the HR was below 50% of the user's max HR</value>
        public int? UnderHealthyHeart { get; set; }
        /// <value>Populated for backwards compatibility until V2 The number of minutes where the HR was below 50% of the user's max HR</value>
        [Obsolete("This field is deprecated. The correct field name is now UnderHealthyHeart.")]
        public int? UnderAerobic { get; set; }
        /// <value>The number of minutes where the HR was between 70-80% of the user's max HR</value>
        public int? Aerobic { get; set; }
        /// <value>The number of minutes where the HR was between 80-90% of the user's max HR</value>
        public int? Anaerobic { get; set; }
        /// <value>The number of minutes where the HR was between 60-70% of the user's max HR</value>
        public int? FitnessZone { get; set; }
        /// <value>The number of minutes where the HR was between 50-60% of the user's max HR</value>
        public int? HealthyHeart { get; set; }
        /// <value>The number of minutes where the HR was between 90-100% of the user's max HR</value>
        public int? Redline { get; set; }
        /// <value>The number of minutes above the user's max HR</value>
        public int? OverRedline { get; set; }
    }
}
