
namespace HealthSdkPrototype
{
    internal static partial class AppSettingsExtensionsArgs
    {
        private enum State
        {
            ExpectOption,
            GetClientSecret,
        }

        public static AppSettings FromArgs(this AppSettings result, string[] args)
        {
            var state = State.ExpectOption;

            foreach (var arg in args)
            {
                switch (state)
                {
                    case State.ExpectOption:
                        if (arg.Length > 2)
                        {
                            if (arg.StartsWith("-"))
                            {
                                var option = arg.TrimStart('-').ToLowerInvariant();

                                switch (option)
                                {
                                    case "secret":
                                        state = State.GetClientSecret;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        break;
                    case State.GetClientSecret:
                        result.ClientSecret = arg;
                        state = State.ExpectOption;
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
    }
}
