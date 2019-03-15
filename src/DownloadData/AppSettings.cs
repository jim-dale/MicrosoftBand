
namespace HealthSdkPrototype
{
    using Newtonsoft.Json;

    public class AppSettings
    {
        // Oauth2 parameters
        public string ClientId { get; set; }
        [JsonIgnore]
        public string ClientSecret { get; set; }
        public string ListenUri { get; set; }
        public string Scope { get; set; }

        // Application instance settings
        public string OutputPath { get; set; }
        public bool OutputFolderPerType { get; set; }
    }
}
