using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using HealthCloudClient;
using Serilog;

namespace HealthSdkPrototype
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {
            int result = 0;

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs\\HealthSdkPrototype-.log", rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                await MainAsync(args);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly");
                result = -1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
            return result;
        }

        private static async Task MainAsync(string[] args)
        {
            var settings = new AppSettings()
                .FromAppDefault()
                .FromConfiguration(ConfigurationManager.AppSettings)
                .FromEnvironmentVariable("MS_HEALTHAPI_CLIENTSECRET")
                .FromJsonFile(optional: true)
                .Initialise();
            Log.Information("AppSettings:{@AppSettings}", settings);

            var state = new AppState()
                .FromJsonFile(optional: true);
            Log.Information("State:{@State}", state);

            Log.Information("Retrieving cached access token");
            if (TryGetCachedAccessToken(state, out AccessToken accessToken) == false)
            {
                Log.Information("Cached access token is either not available or has expired");

                Log.Information("Authenticating application and waiting for new access token");
                accessToken = await GetAccessToken(settings, state);
            }
            if (accessToken != null)
            {
                var healthClient = new HealthClient(accessToken);
                Log.Information("Access Token:{@AccessToken}", accessToken);

                Log.Information("Getting user profile");
                var profile = healthClient.GetProfile();
                Log.Information("User Profile:{@UserProfile}", profile);

                if (state.AccountCreatedTimeUtc == DateTime.MinValue)
                {
                    state.AccountCreatedTimeUtc = DateTime.UtcNow;

                    if (profile.CreatedTime.HasValue)
                    {
                        state.AccountCreatedTimeUtc = profile.CreatedTime.Value.ToUniversalTime();
                    }
                    Log.Information("Set AccountCreatedTimeUtc in application state to {AccountCreatedTimeUtc}", state.AccountCreatedTimeUtc);
                }
                if (profile.LastUpdateTime.HasValue)
                {
                    state.AccountLastUpdateTimeUtc = profile.LastUpdateTime.Value.ToUniversalTime();
                    Log.Information("Set AccountLastUpdateTimeUtc in application state to {AccountLastUpdateTimeUtc}", state.AccountLastUpdateTimeUtc);
                }
                if (state.LastDailySummarySyncTimeUtc == DateTime.MinValue)
                {
                    state.LastDailySummarySyncTimeUtc = state.AccountCreatedTimeUtc;
                    Log.Information("Set LastDailySummarySyncTimeUtc in application state to {LastDailySummarySyncTimeUtc}", state.LastDailySummarySyncTimeUtc);
                }
                if (state.LastHourlySummarySyncTimeUtc == DateTime.MinValue)
                {
                    state.LastHourlySummarySyncTimeUtc = state.AccountCreatedTimeUtc;
                    Log.Information("Set LastHourlySummarySyncTimeUtc in application state to {LastHourlySummarySyncTimeUtc}", state.LastHourlySummarySyncTimeUtc);
                }
                if (state.LastDailyActivitiesSyncTimeUtc == DateTime.MinValue)
                {
                    state.LastDailyActivitiesSyncTimeUtc = state.AccountCreatedTimeUtc;
                    Log.Information("Set LastDailyActivitiesSyncTimeUtc in application state to {LastDailyActivitiesSyncTimeUtc}", state.LastDailyActivitiesSyncTimeUtc);
                }

                SyncDailySummaries(settings, state, healthClient);
                SyncHourlySummaries(settings, state, healthClient);
                SyncActivities(settings, state, healthClient);

                Log.Information("Saving application state");
                state.ToJson();
            }
        }

        private static void SyncDailySummaries(AppSettings settings, AppState state, HealthClient healthClient)
        {
            var startDate = state.LastDailySummarySyncTimeUtc;
            var endDate = state.AccountLastUpdateTimeUtc;

            Log.Information("SyncDailySummaries/Start Date:{StartDate}/End Date:{EndDate}", startDate, endDate);

            if (endDate > startDate)
            {
                SyncSummaries(settings, healthClient, Period.Daily, startDate, endDate);

                state.LastDailySummarySyncTimeUtc = state.AccountLastUpdateTimeUtc;
                Log.Information("Set LastDailySummarySyncTimeUtc in application state to {LastDailySummarySyncTimeUtc}", state.LastDailySummarySyncTimeUtc);
            }
        }

        private static void SyncHourlySummaries(AppSettings settings, AppState state, HealthClient healthClient)
        {
            var startDate = state.LastHourlySummarySyncTimeUtc;
            var endDate = state.AccountLastUpdateTimeUtc;

            Log.Information("SyncHourlySummaries/Start Date:{StartDate}/End Date:{EndDate}", startDate, endDate);

            if (endDate > startDate)
            {
                SyncSummaries(settings, healthClient, Period.Hourly, startDate, endDate);

                state.LastHourlySummarySyncTimeUtc = state.AccountLastUpdateTimeUtc;
                Log.Information("Set LastHourlySummarySyncTimeUtc in application state to {LastHourlySummarySyncTimeUtc}", state.LastHourlySummarySyncTimeUtc);
            }
        }

        private static void SyncActivities(AppSettings settings, AppState state, HealthClient healthClient)
        {
            var startDate = state.LastDailyActivitiesSyncTimeUtc;
            var endDate = state.AccountLastUpdateTimeUtc;

            Log.Information("SyncActivities/Start Date:{StartDate}/End Date:{EndDate}", startDate, endDate);

            if (endDate > startDate)
            {
                SyncActivities(settings, healthClient, startDate, endDate);

                state.LastDailyActivitiesSyncTimeUtc = state.AccountLastUpdateTimeUtc;
                Log.Information("Set LastDailyActivitiesSyncTimeUtc in application state to {LastDailyActivitiesSyncTimeUtc}", state.LastDailyActivitiesSyncTimeUtc);
            }
        }

        private static void SyncActivities(AppSettings settings, HealthClient healthClient, DateTime startDate, DateTime endDate)
        {
            startDate.ForEachDay(endDate,
                (state, day) =>
                {
                    using (Log.Logger.BeginTimedOperation("Getting Activities", day.ToString("u")))
                    {
                        string json = GetActivitiesForDay(state, day);
                        SaveJsonForDay(settings, day, json, "Activities");
                    }
                },
                healthClient);
        }

        private static void SyncSummaries(AppSettings settings, HealthClient healthClient, Period period, DateTime startDate, DateTime endDate)
        {
            startDate.ForEachDay(endDate,
                (state, day) =>
                {
                    using (Log.Logger.BeginTimedOperation("Getting Summary", day.ToString("u")))
                    {
                        var json = GetSummaryForDay(state, period, day);

                        SaveJsonForDay(settings, day, json, $"{period} summary");
                    }
                },
                healthClient);
        }

        private static string GetSummaryForDay(HealthClient healthClient, Period period, DateTime day)
        {
            DateTime startDate = day.Date;
            DateTime endDate = startDate.AddDays(1);

            var result = healthClient.GetSummariesAsJson(period, startTime: startDate, endTime: endDate);

            return result;
        }

        private static string GetActivitiesForDay(HealthClient healthClient, DateTime day)
        {
            DateTime startDate = day.Date;
            DateTime endDate = startDate.AddDays(1);

            var result = healthClient.GetActivitiesAsJson(startTime: startDate, endTime: endDate, activityIncludes: HealthClient.AllActivityDetails);

            return result;
        }

        private static void SaveJsonForDay(AppSettings settings, DateTime day, string json, string suffix)
        {
            if (String.IsNullOrWhiteSpace(json) == false)
            {
                string path = $"{day:yyyy-MM-dd}-{suffix}.json";

                if (String.IsNullOrWhiteSpace(settings.OutputPath) == false)
                {
                    string folder = Path.GetFullPath(settings.OutputPath);

                    if (settings.OutputFolderPerType)
                    {
                        folder = Path.Combine(folder, suffix);
                    }
                    Directory.CreateDirectory(folder);

                    path = Path.Combine(folder, path);
                }
                File.WriteAllText(path, json);
            }
        }

        private static bool TryGetCachedAccessToken(AppState state, out AccessToken result)
        {
            bool success = true;

            result = state.AccessToken;
            if (result is null || state.AccessTokenExpirationTimeUtc < DateTime.UtcNow)
            {
                result = null;
                success = false;
            }

            return success;
        }

        private static async Task<AccessToken> GetAccessToken(AppSettings settings, AppState state)
        {
            var oauthClient = new Oauth2Client
            {
                ClientId = settings.ClientId,
                ClientSecret = settings.ClientSecret,
                RedirectUrl = settings.ListenUri,
                Scope = settings.Scope,
            };

            var result = await oauthClient.GetAccessTokenAsync();
            if (result != null)
            {
                state.AccessTokenExpirationTimeUtc = DateTime.UtcNow.AddSeconds(result.ExpiresIn);
                state.AccessToken = result;
            }

            return result;
        }
    }
}
