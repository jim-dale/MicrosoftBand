
namespace HealthSdkPrototype
{
    using System;
    using System.IO;
    using Newtonsoft.Json;

    internal static partial class AppSettingsExtensionsJson
    {
        private const string DefaultSettingsFileName = "appsettings.json";
        private static readonly JsonSerializerSettings _jsSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

        public static AppSettings FromJsonFile(this AppSettings result, string path = DefaultSettingsFileName, bool optional = false)
        {
            var ignore = String.IsNullOrWhiteSpace(path) || (optional && File.Exists(path) == false);
            if (ignore == false)
            {
                var json = File.ReadAllText(path);
                JsonConvert.PopulateObject(json, result, _jsSettings);
            }

            return result;
        }
    }
}
