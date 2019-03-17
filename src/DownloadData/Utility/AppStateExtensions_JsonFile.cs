
namespace HealthSdkPrototype
{
    using System;
    using System.IO;
    using Newtonsoft.Json;

    internal static partial class AppStateExtensions
    {
        private const string DefaultFileName = "downloaddata.appstate.json";
        private static readonly JsonSerializerSettings _jsSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

        public static AppState FromJsonFile(this AppState result, string path = DefaultFileName, bool optional = false)
        {
            var ignore = String.IsNullOrWhiteSpace(path) || (optional && File.Exists(path) == false);
            if (ignore == false)
            {
                var json = File.ReadAllText(path);
                JsonConvert.PopulateObject(json, result, _jsSettings);
            }

            return result;
        }

        public static void ToJson(this AppState result, string path = DefaultFileName)
        {
            var json = JsonConvert.SerializeObject(result, Formatting.Indented, _jsSettings);
            File.WriteAllText(path, json);
        }
    }
}
