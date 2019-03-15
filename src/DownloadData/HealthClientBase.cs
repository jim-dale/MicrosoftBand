
namespace HealthSdkPrototype
{
    using System;
    using System.Diagnostics;
    using HealthCloudClient;
    using Newtonsoft.Json;
    using RestSharp;
    using RestSharp.Authenticators;

    public class HealthClientBase
    {
        public const ActivityIncludes AllActivityDetails = ActivityIncludes.Details | ActivityIncludes.MapPoints | ActivityIncludes.MinuteSummaries;

        public IRestClient Client { get; }

        protected const string BaseUrl = "https://api.microsofthealth.net/";
        protected const string ProfilePath = "v1/me/Profile";
        protected const string DevicesPath = "v1/me/Devices";
        protected const string ActivitiesPath = "v1/me/Activities";
        protected const string SummariesPath = "v1/me/Summaries";

        protected IRestClient _pageClient;

        private readonly IAuthenticator _authenticator;
        private readonly JsonConverter _timeSpanConverter;

        protected HealthClientBase(AccessToken accessToken)
        {
            if (accessToken == null)
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            _authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(accessToken.Token, accessToken.TokenType);
            _timeSpanConverter = new Iso8601DurationConverter();

            Client = CreateRestClient(BaseUrl);
        }

        protected IRestClient GetPageClient(string url)
        {
            if (_pageClient is null)
            {
                _pageClient = CreateRestClient(url);
            }
            return _pageClient;
        }

        /// <summary>
        /// Serialise an object to a JSON string
        /// </summary>
        /// <param name="value">Object to serialise as JSON</param>
        /// <returns>JSON string</returns>
        public string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value, _timeSpanConverter);
        }

        /// <summary>
        /// Deserialise a JSON string to an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">JSON string to deserialise</param>
        /// <returns></returns>
        public T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value, _timeSpanConverter);
        }

        protected static RestRequest CreateActivitiesRequest(DateTime startTime, DateTime? endTime, string activityIds, string activityTypes, ActivityIncludes activityIncludes, string deviceIds, SplitDistanceType? splitDistanceType, int? maxPageSize)
        {
            var request = new RestRequest(ActivitiesPath, Method.GET);

            // Add required parameter
            request.AddParameter("startTime", startTime.ToString("o"));

            // Add optional parameters
            if (endTime.HasValue)
            {
                request.AddParameter("endTime", endTime.Value.ToString("o"));
            }
            if (String.IsNullOrWhiteSpace(activityIds) == false)
            {
                request.AddParameter("activityIds", activityIds);
            }
            if (String.IsNullOrWhiteSpace(activityTypes) == false)
            {
                request.AddParameter("activityTypes", activityTypes);
            }
            if (activityIncludes != ActivityIncludes.None)
            {
                request.AddParameter("activityIncludes", activityIncludes.ToString());
            }
            if (String.IsNullOrWhiteSpace(deviceIds) == false)
            {
                request.AddParameter("deviceIds", deviceIds);
            }
            if (splitDistanceType.HasValue)
            {
                request.AddParameter("splitDistanceType", splitDistanceType.Value);
            }
            if (maxPageSize.HasValue)
            {
                request.AddParameter("maxPageSize", maxPageSize.Value);
            }

            return request;
        }

        protected static RestRequest CreateActivityRequest(string activityId, ActivityIncludes activityIncludes)
        {
            var request = new RestRequest($"{ActivitiesPath}/{activityId}", Method.GET);

            // Add optional parameter
            if (activityIncludes != ActivityIncludes.None)
            {
                request.AddParameter("activityIncludes", activityIncludes.ToString());
            }

            return request;
        }

        protected static RestRequest CreateSummariesRequest(Period period, DateTime startTime, DateTime? endTime, string deviceIds, int? maxPageSize)
        {
            var request = new RestRequest($"{SummariesPath}/{period}", Method.GET);

            // Add required parameter
            request.AddParameter("startTime", startTime.ToString("o"));

            // Add optional parameters
            if (endTime.HasValue)
            {
                request.AddParameter("endTime", endTime.Value.ToString("o"));
            }
            if (String.IsNullOrWhiteSpace(deviceIds) == false)
            {
                request.AddParameter("deviceIds", deviceIds);
            }
            if (maxPageSize.HasValue)
            {
                request.AddParameter("maxPageSize", maxPageSize.Value);
            }

            return request;
        }

        protected IRestResponse GetResponse(IRestRequest request)
        {
            return GetResponse(Client, request);
        }

        protected IRestResponse GetResponse(IRestClient client, IRestRequest request)
        {
            var sw = Stopwatch.StartNew();
            var result = client.Execute(request);
            sw.Stop();

            RestSharpLogger.LogRequest(client, request, result, sw.ElapsedMilliseconds);
            RestSharpLogger.LogResponseContent(result);

            return result;
        }

        protected T GetItem<T>(string resource)
        {
            return GetItem<T>(new RestRequest(resource, Method.GET));
        }

        protected T GetItem<T>(IRestRequest request)
        {
            var result = default(T);

            var response = GetResponse(request);
            if (response.IsSuccessful)
            {
                if (typeof(T) == typeof(String))
                {
                    result = (T)Convert.ChangeType(response.Content, typeof(String));
                }
                else
                {
                    result = DeserializeObject<T>(response.Content);
                }
            }

            return result;
        }

        #region Private methods
        private IRestClient CreateRestClient(string baseUrl)
        {
            return new RestClient(baseUrl)
            {
                Authenticator = _authenticator,
            };
        }
        #endregion
    }
}
