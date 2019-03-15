
namespace HealthSdkPrototype
{
    using System;
    using System.IO;

    internal static partial class AppSettingsExtensions
    {
        public static AppSettings FromAppDefault(this AppSettings result)
        {
            result.Scope = "mshealth.ReadProfile mshealth.ReadDevices mshealth.ReadActivityHistory mshealth.ReadActivityLocation offline_access";
            result.OutputFolderPerType = true;

            return result;
        }

        public static AppSettings Initialise(this AppSettings result)
        {
            if (String.IsNullOrWhiteSpace(result.OutputPath))
            {
                result.OutputPath = Directory.GetCurrentDirectory();
            }
            else
            {
                result.OutputPath = Environment.ExpandEnvironmentVariables(result.OutputPath);

                if (Directory.Exists(result.OutputPath) == false)
                {
                    Directory.CreateDirectory(result.OutputPath);
                }
            }

            return result;
        }
    }
}
