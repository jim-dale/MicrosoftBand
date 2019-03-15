
namespace HealthSdkPrototype
{
    using System;
    using HealthCloudClient;
    using RestSharp;

    public class HealthClient : HealthClientBase
    {
        public HealthClient(AccessToken accessToken)
            : base(accessToken)
        {
        }

        /// <summary>
        /// Get the user's Microsoft Health profile.
        /// </summary>
        /// <returns>The user profile or null if the user profile is not available.</returns>
        public UserProfile GetProfile()
        {
            return GetItem<UserProfile>(ProfilePath);
        }

        /// <summary>
        /// Get the details about the devices associated with the user's Microsoft Health profile.
        /// </summary>
        /// <returns>The devices response object or null if the response is not available.</returns>
        public DevicesResponse GetDevices()
        {
            return GetItem<DevicesResponse>(DevicesPath);
        }

        /// <summary>
        /// Get device details.
        /// </summary>
        /// <param name="deviceId">The id of the device</param>
        /// <returns>The device profile associated with the given id, null if the id is not found.</returns>
        public DeviceProfile GetDevice(string deviceId)
        {
            var request = new RestRequest($"{DevicesPath}/{deviceId}", Method.GET);

            return GetItem<DeviceProfile>(request);
        }

        /// <summary>
        /// Get a collection of activities.
        /// </summary>
        /// <param name="startTime">Filters the set of returned activities to those starting after the specified start time, inclusive.</param>
        /// <param name="endTime">Filters the set of returned activities to those starting before the specified end time, exclusive.</param>
        /// <param name="activityIds">The comma-separated list of activity ids to return. Defaults to all activities.</param>
        /// <param name="activityTypes">The comma-separated list of activity types to return. Defaults to all activity types.</param>
        /// <param name="activityIncludes">The comma-separated list of additional properties to return: Details, MinuteSummaries, MapPoints.Defaults to none.</param>
        /// <param name="deviceIds">Filters the set of returned activities based on the comma-separated list of device ids provided. Defaults to no filter.</param>
        /// <param name="splitDistanceType">The length of splits used in each activity. Defaults to Mile.</param>
        /// <param name="maxPageSize">The maximum number of entries to return per page. Defaults to 1000.</param>
        /// <returns>A collection of activities or null if no activities are available</returns>
        public ActivitiesResponse GetActivities(DateTime startTime, DateTime? endTime = null,
            string activityIds = null, string activityTypes = null, ActivityIncludes activityIncludes = ActivityIncludes.None,
            string deviceIds = null, SplitDistanceType? splitDistanceType = SplitDistanceType.Kilometer, int? maxPageSize = null)
        {
            RestRequest request = CreateActivitiesRequest(startTime, endTime, activityIds, activityTypes, activityIncludes, deviceIds, splitDistanceType, maxPageSize);

            return GetItem<ActivitiesResponse>(request);
        }

        /// <summary>
        /// <see cref="GetActivities(DateTime, DateTime?, string, string, ActivityIncludes, string, SplitDistanceType?, int?)"/>
        /// </summary>
        public string GetActivitiesAsJson(DateTime startTime, DateTime? endTime = null,
            string activityIds = null, string activityTypes = null, ActivityIncludes activityIncludes = ActivityIncludes.None,
            string deviceIds = null, SplitDistanceType? splitDistanceType = SplitDistanceType.Kilometer, int? maxPageSize = null)
        {
            RestRequest request = CreateActivitiesRequest(startTime, endTime, activityIds, activityTypes, activityIncludes, deviceIds, splitDistanceType, maxPageSize);

            return GetItem<string>(request);
        }

        /// <summary>
        /// Get the details of an activity by activity id.
        /// </summary>
        /// <param name="activityId">The id of the activity to get.</param>
        /// <param name="activityIncludes">The comma-separated list of additional properties to return: MinuteSummaries, MapPoints. Defaults to none.</param>
        /// <returns>A collection of activities or null if no activities are available</returns>
        public ActivitiesResponse GetActivity(string activityId, ActivityIncludes activityIncludes = ActivityIncludes.None)
        {
            if (string.IsNullOrWhiteSpace(activityId))
            {
                throw new ArgumentException("Must supply an activity id", nameof(activityId));
            }

            var request = CreateActivityRequest(activityId, activityIncludes);

            return GetItem<ActivitiesResponse>(request);
        }

        /// <summary>
        /// Get a collection of activity summaries.
        /// </summary>
        /// <param name="period">The time period for each summary, Daily or Hourly.</param>
        /// <param name="startTime">The start time of the summaries, inclusive.</param>
        /// <param name="endTime">The end time of the summaries, exclusive. Defaults to the current time in UTC.date-time.</param>
        /// <param name="deviceIds">The comma-separated list of device ids that generated the data. Defaults to the aggregation of all devices.</param>
        /// <param name="maxPageSize">The maximum number of entries to return per page. Defaults to 48 for hourly and 31 for daily.</param>
        public SummaryResponse GetSummaries(Period period, DateTime startTime, DateTime? endTime = null, string deviceIds = null, int? maxPageSize = null)
        {
            RestRequest request = CreateSummariesRequest(period, startTime, endTime, deviceIds, maxPageSize);

            return GetItem<SummaryResponse>(request);
        }

        /// <summary>
        /// <see cref="GetSummaries(Period, DateTime, DateTime?, string, int?)"/>
        /// </summary>
        public string GetSummariesAsJson(Period period, DateTime startTime, DateTime? endTime = null, string deviceIds = null, int? maxPageSize = null)
        {
            RestRequest request = CreateSummariesRequest(period, startTime, endTime, deviceIds, maxPageSize);

            return GetItem<string>(request);
        }

        /// <summary>
        /// Get the next page of results.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nextPage"></param>
        /// <returns></returns>
        public T GetNextPage<T>(string nextPageUrl)
        {
            var result = default(T);

            var client = GetPageClient(nextPageUrl);
            var request = new RestRequest(Method.GET);

            var response = GetResponse(client, request);
            if (response.IsSuccessful)
            {
                result = DeserializeObject<T>(response.Content);
            }
            return result;
        }
    }
}
