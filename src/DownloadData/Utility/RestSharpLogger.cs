
namespace HealthSdkPrototype
{
    using System.Linq;
    using RestSharp;
    using Serilog;

    public static class RestSharpLogger
    {
        public static void LogRequest(IRestClient restClient, IRestRequest request, IRestResponse response, long durationMs)
        {
            var requestToLog = new
            {
                resource = request.Resource,
                // Parameters are custom anonymous objects in order to have the parameter type as a nice string
                // otherwise it will just show the enum value
                parameters = request.Parameters.Select(parameter => new
                {
                    name = parameter.Name,
                    value = parameter.Value,
                    type = parameter.Type.ToString()
                }),
                // ToString() here to have the method as a nice string otherwise it will just show the enum value
                method = request.Method.ToString(),
                // This will generate the actual Uri used in the request
                uri = restClient.BuildUri(request),
            };

            var responseToLog = new
            {
                statusCode = response.StatusCode,
                content = response.Content,
                headers = response.Headers,
                // The Uri that actually responded (could be different from the requestUri if a redirection occurred)
                responseUri = response.ResponseUri,
                errorMessage = response.ErrorMessage,
            };

            Log.Information("RestClient/Request completed in {DurationMs}ms", durationMs);
            Log.Information("RestClient/Request:{@Request}", requestToLog);
            Log.Information("RestClient/Response:{@Response}", responseToLog);
        }

        public static void LogResponseContent(IRestResponse response)
        {
            Log.Information("RestClient/Content:{@Content}", response.Content);
        }
    }
}
