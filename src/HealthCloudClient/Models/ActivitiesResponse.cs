
namespace HealthCloudClient
{
    /// <summary>
    /// Container for a list of Activity records returned by the REST API
    /// </summary>
    public class ActivitiesResponse
    {
        /// <value>The collection of bike activities</value>
        public BikeActivity[] BikeActivities { get; set; }
        /// <value>The collection of hike activities</value>
        public HikeActivity[] HikeActivities { get; set; }
        /// <value>The collection of free play activities</value>
        public FreePlayActivity[] FreePlayActivities { get; set; }
        /// <value>The collection of golf activities</value>
        public GolfActivity[] GolfActivities { get; set; }
        /// <value>The collection of guided workout activities</value>
        public GuidedWorkoutActivity[] GuidedWorkoutActivities { get; set; }
        /// <value>The collection of run activities</value>
        public RunActivity[] RunActivities { get; set; }
        /// <value>The collection of sleep activities</value>
        public SleepActivity[] SleepActivities { get; set; }
        /// <value>The URI for the next page of data</value>
        public string NextPage { get; set; }
        /// <value>The number of items returned</value>
        public int? ItemCount { get; set; }
    }
}
