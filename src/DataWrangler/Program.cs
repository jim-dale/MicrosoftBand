using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HealthCloudClient;
using Newtonsoft.Json;

namespace DataWrangler
{
    class Program
    {
        static void Main(string[] args)
        {
            //var summaries = GetAllHourlySummaries();
            //var items = GetCaloriesByHourOfEachDay(summaries);
            //var items = GetDistanceByHourByDayOfWeek(summaries);

            //SaveAsTsv(items, "data2.tsv");


            //string json = JsonConvert.SerializeObject(items);


            var dailySummaries = GetAllDailySummaries();
            var items = GetDistanceByDay(dailySummaries);
            SaveAsTsv(items, @"D:\tmp\data3.tsv");
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

        private static List<Summary> GetAllDailySummaries()
        {
            var path = @"%WORK_DRIVE%%HOMEPATH%\Documents\Documents\Exercise\DailySummaries";
            path = Environment.ExpandEnvironmentVariables(path);

            var files = Directory.EnumerateFiles(path);

            var query = from file in files
                        let s = GetSummary(file)
                        select s;
            return query.ToList();
        }

        private static List<Summary> GetAllHourlySummaries()
        {
            var path = @"%WORK_DRIVE%%HOMEPATH%\Documents\Documents\Exercise\HourlySummaries";
            path = Environment.ExpandEnvironmentVariables(path);

            var files = Directory.EnumerateFiles(path);

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

    internal class DistanceByDay
    {
        public string date { get; set; }
        public int? value { get; set; }
    }

    internal class CaloriesByHourEachDay
    {
        public int day { get; set; }
        public int hour { get; set; }
        public int value { get; set; }
    }

    internal class StepsByMonth
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int? StepsTaken { get; set; }
    }
}
