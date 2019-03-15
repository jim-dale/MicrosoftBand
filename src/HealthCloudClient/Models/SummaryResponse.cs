
namespace HealthCloudClient
{
    /// <summary>
    /// Container for a list of Summary records returned by the REST API
    /// </summary>
    public class SummaryResponse
    {
        /// <value>The collection of summaries</value>
        public Summary[] Summaries { get; set; }
        /// <value>The URI for the next page of data</value>
        public string NextPage { get; set; }
        /// <value>The number of items returned</value>
        public int? ItemCount { get; set; }
    }
}
