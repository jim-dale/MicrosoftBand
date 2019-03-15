
namespace HealthSdkPrototype
{
    using System;

    internal static partial class AppSettingsExtensions
    {
        public static AppSettings FromEnvironmentVariable(this AppSettings result, string name)
        {
            result.ClientSecret = Environment.GetEnvironmentVariable(name);

            return result;
        }
    }
}
