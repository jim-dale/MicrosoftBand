
namespace HealthSdkPrototype
{
    using System;
    using System.Diagnostics;
    using System.Threading.Channels;
    using System.Threading.Tasks;
    using HealthCloudClient;
    using Newtonsoft.Json;
    using RestSharp;
    using Serilog;

    public partial class Oauth2Client
    {
        private const string OauthBaseUri = "https://login.live.com";
        private const string OauthAuthorizePath = "oauth20_authorize.srf";
        private const string OauthTokenPath = "oauth20_token.srf";

        public string RedirectUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }

        public async Task<AccessToken> GetAccessTokenAsync()
        {
            var result = default(AccessToken);

            var authorisationCode = await GetAuthorisationCodeAsync();
            if (String.IsNullOrWhiteSpace(authorisationCode) == false)
            {
                result = GetAccessToken(authorisationCode);
            }
            return result;
        }

        private async Task<string> GetAuthorisationCodeAsync()
        {
            string result = null;

            // Create a channel to communicate safetly between the web server thread and this thread
            var channel = Channel.CreateUnbounded<Message>(new UnboundedChannelOptions()
            {
                SingleWriter = true,
                SingleReader = true
            });

            // Create web server and start it. The server will wait for the Oauth2 response message with the authorisation code
            // and post the code to the channel for the reader to pick up.
            using (var server = new OauthWebServer())
            {
                server.Writer = channel.Writer;
                server.Start(RedirectUrl);

                // Start a web browser to allow user to authenticate and get user authorisation
                // This returns a code via the web server above.
                StartAuthorisationRequest();

                // Wait for the web server to send an authorisation code message
                var reader = channel.Reader;
                var message = await reader.ReadAsync();

                if (String.IsNullOrWhiteSpace(message.Code) == false)
                {
                    result = message.Code;
                    Log.Information("GetAuthorisationCodeAsync/Authorisation code:\"{AuthorisationCode}\"", result);
                }
                else if (message.Exception != null)
                {
                    Log.Error(message.Exception, String.Empty);

                    if ((uint)message.Exception.HResult == 0x8000_4005)
                    {
                        Log.Information("This error is probably caused by the listener URL not being authorised.");
                        Log.Information("Try authorising the URL from an administator command prompt by running the following command.");
                        Log.Information("netsh http add urlacl url=\"{Uri}\" user=DOMAIN\\user", RedirectUrl);
                    }
                }

                server.Stop();
            }

            return result;
        }

        private AccessToken GetAccessToken(string code)
        {
            var result = default(AccessToken);

            if (String.IsNullOrWhiteSpace(code) == false)
            {
                var request = new RestRequest(OauthTokenPath, Method.POST);
                request.AddParameter("redirect_uri", RedirectUrl);
                request.AddParameter("client_id", ClientId);
                request.AddParameter("client_secret", ClientSecret);
                request.AddParameter("code", code);
                request.AddParameter("grant_type", "authorization_code");

                var client = new RestClient(OauthBaseUri);

                var sw = Stopwatch.StartNew();
                var response = client.Execute(request);
                sw.Stop();

                RestSharpLogger.LogRequest(client, request, response, sw.ElapsedMilliseconds);
                RestSharpLogger.LogResponseContent(response);

                if (response.IsSuccessful)
                {
                    result = JsonConvert.DeserializeObject<AccessToken>(response.Content);
                }
                else
                {
                    if (response.ErrorException is null)
                    {
                        Log.Error(response.ErrorMessage);
                    }
                    else
                    {
                        Log.Error(response.ErrorException, response.ErrorMessage);
                    }
                }
            }

            return result;
        }

        private void StartAuthorisationRequest()
        {
            string url = GetAuthorisationRequestUrl();

            _ = Process.Start(url);
            Log.Information("Process.Start/Url:\"{url}\"", url);
        }

        private string GetAuthorisationRequestUrl()
        {
            UriBuilder builder = new UriBuilder(OauthBaseUri);

            builder.Path = OauthAuthorizePath;
            builder.Query = $"redirect_uri={RedirectUrl}&client_id={ClientId}&scope={Scope}&response_type=code";

            return builder.ToString();
        }

        private const string CloseWindowResponse = "<!DOCTYPE html><html><head></head><body onload=\"closeThis();\"><h1>Authorization Successfull</h1><p>You can now close this window</p><script type=\"text/javascript\">function closeMe() { window.close(); } function closeThis() { window.close(); }</script></body></html>";
    }
}
