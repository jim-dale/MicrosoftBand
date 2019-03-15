
namespace HealthSdkPrototype
{
    using System.Collections.Specialized;

    internal static partial class AppSettingsExtensions
    {
        public static AppSettings FromConfiguration(this AppSettings result, NameValueCollection settings)
        {
            result.ClientId = settings.GetAs(result.ClientId, nameof(result.ClientId));
            result.ListenUri = settings.GetAs(result.ListenUri, nameof(result.ListenUri));
            result.Scope = settings.GetAs(result.Scope, nameof(result.Scope));

            return result;
        }
    }
}
