using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HealthCloudClient;
using Microsoft.Extensions.FileSystemGlobbing;
using Newtonsoft.Json;

namespace DataWrangler
{
    internal class Program
    {
        private static JsonConverter _timeSpanConverter;

        private static void Main(string[] args)
        {
            _timeSpanConverter = new Iso8601DurationConverter();

            var basePath = Directory.GetCurrentDirectory();

            //var path = Path.Combine(basePath, "Daily Summary");
            var path = Path.Combine(basePath, "Activities");
            var matcher = new Matcher();
            matcher.AddInclude("**/*.json");

            var files = matcher.GetResultsInFullPath(path);

            int totalBikeRides = 0;
            int totalHikes = 0;

            foreach (var file in files)
            {
                var str = File.ReadAllText(file);
                var activities = JsonConvert.DeserializeObject<ActivitiesResponse>(str, _timeSpanConverter);

                if (activities?.BikeActivities != null && activities.BikeActivities.Any())
                {
                    totalBikeRides += activities.BikeActivities.Length;
                }
                if (activities?.HikeActivities != null && activities.HikeActivities.Any())
                {
                    totalHikes += activities.HikeActivities.Length;

                    var hike = activities.HikeActivities[0];

                    if (hike.MapPoints != null)
                    {
                        var points = from mp in hike.MapPoints
                                     where mp.Location != null
                                     select new
                                     {
                                         mp.MapPointType,
                                         mp.Location.Latitude,
                                         mp.Location.Longitude,
                                     };

                        foreach (var point in points)
                        {
                            Console.WriteLine($"{point.MapPointType},{point.Latitude},{point.Longitude}");
                        }
                    }
                    break;
                }
            }

            Console.WriteLine($"{totalBikeRides},{totalHikes}");
            //var dailySummaries = GetAllDailySummaries(files);

            //var summaries = GetAllHourlySummaries();
            //var items = GetCaloriesByHourOfEachDay(summaries);
            //var items = GetDistanceByHourByDayOfWeek(summaries);

            //SaveAsTsv(items, "data2.tsv");

            //var items = GetDistanceByDay(dailySummaries);

            //SaveAsTsv(items, @"data3.tsv");
        }

        private static void SaveAsTsv(IEnumerable<CaloriesByHourEachDay> items, string path)
        {
            using (var fs = new StreamWriter(path))
            {
                fs.WriteLine("day\thour\tvalue");
                foreach (var item in items)
                {
                    fs.WriteLine($"{item.day}\t{item.hour}\t{item.value}");
                }
            }
        }

        private static void SaveAsTsv(IEnumerable<DistanceByDay> items, string path)
        {
            using (var fs = new StreamWriter(path))
            {
                fs.WriteLine("date\tvalue");
                foreach (var item in items)
                {
                    fs.WriteLine($"{item.date}\t{item.value}");
                }
            }
        }

        private static IEnumerable<DistanceByDay> GetDistanceByDay(IEnumerable<Summary> summaries)
        {
            var query = from s in summaries
                        orderby s.ParentDay.Value
                        select new DistanceByDay
                        {
                            date = s.ParentDay.Value.ToString("yyyy-MM-dd"),
                            value = s.DistanceSummary.TotalDistance ?? 0,
                        };

            return query;
        }

        private static IEnumerable<CaloriesByHourEachDay> GetCaloriesByHourByDayOfWeek(IEnumerable<Summary> summaries)
        {
            var query = from s in summaries
                        group s by new { DayOfWeek = (int)s.StartTime.Value.DayOfWeek, s.StartTime.Value.Hour } into g
                        orderby g.Key.DayOfWeek, g.Key.Hour
                        select new CaloriesByHourEachDay
                        {
                            day = g.Key.DayOfWeek + 1,
                            hour = g.Key.Hour + 1,
                            value = g.Sum(i => i.CaloriesBurnedSummary.TotalCalories ?? 0),
                        };

            return query;
        }

        private static IEnumerable<CaloriesByHourEachDay> GetDistanceByHourByDayOfWeek(IEnumerable<Summary> summaries)
        {
            var query = from s in summaries
                        group s by new { DayOfWeek = (int)s.StartTime.Value.DayOfWeek, s.StartTime.Value.Hour } into g
                        orderby g.Key.DayOfWeek, g.Key.Hour
                        select new CaloriesByHourEachDay
                        {
                            day = g.Key.DayOfWeek + 1,
                            hour = g.Key.Hour + 1,
                            value = g.Sum(i => i.DistanceSummary.TotalDistance ?? 0),
                        };

            return query;
        }

        private static IEnumerable<StepsByMonth> GetStepsByMonth(IEnumerable<Summary> summaries)
        {
            var query = from s in summaries
                        group s by new { s.ParentDay.Value.Year, s.ParentDay.Value.Month } into g
                        select new StepsByMonth
                        {
                            Year = g.Key.Year,
                            Month = g.Key.Month,
                            StepsTaken = g.Sum(i => i.StepsTaken),
                        };

            return query;
        }

        private static List<Summary> GetAllDailySummaries(IEnumerable<string> files)
        {
            var query = from file in files
                        let s = GetSummary(file)
                        select s;
            return query.ToList();
        }

        private static List<Summary> GetAllHourlySummaries(IEnumerable<string> files)
        {
            var query = from file in files
                        from s in GetSummaries(file)
                        select s;
            return query.ToList();
        }

        private static Summary[] GetSummaries(string path)
        {
            var json = File.ReadAllText(path);
            var response = JsonConvert.DeserializeObject<SummaryResponse>(json, new Iso8601DurationConverter());
            return response.Summaries;
        }

        private static Summary GetSummary(string path)
        {
            var json = File.ReadAllText(path);
            var response = JsonConvert.DeserializeObject<SummaryResponse>(json, new Iso8601DurationConverter());
            return response.Summaries[0];
        }
    }
}
