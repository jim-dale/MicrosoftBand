
namespace HealthSdkPrototype
{
    using System;

    internal static partial class DateTimeExtensions
    {
        /// <summary>
        /// Executes an action for each day from <paramref name="startDate"/> to <paramref name="endDate"/> inclusive.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="state">State to pass to the action method.</param>
        /// <param name="action">The action to perform for each day.</param>
        /// <returns></returns>
        public static void ForEachDay<TState>(this DateTime startDate, DateTime endDate, Action<TState, DateTime> action, TState state = default(TState))
        {
            var day = startDate.Date;
            var lastDate = endDate.Date;

            while (day <= lastDate)
            {
                action(state, day);

                day = day.AddDays(1);
            }
        }
    }
}
