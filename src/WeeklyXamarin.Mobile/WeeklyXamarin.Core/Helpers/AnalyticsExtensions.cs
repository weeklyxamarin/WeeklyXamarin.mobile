using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WeeklyXamarin.Core.Services;

namespace WeeklyXamarin.Core.Helpers
{
    public static class AnalyticsExtensions
    {
        public static void TrackEvent<T>(this IAnalytics analytics, string eventName, T data)
            where T : class
            => analytics.TrackEvent(eventName, data.ToDictionary());

        private static Dictionary<string, string> ToDictionary<T>(this T obj)
        {
            return obj?.GetType().GetRuntimeProperties()
                           .Select(p =>
                           {
                                object v = null;
                                try { v = p.GetValue(obj); } catch { }

                                return new
                                {
                                    Key = p.Name,
                                    Value = v?.ToString()
                                };
                           })
                           .Where(x => x.Value != null)
                           .GroupBy(x => x.Key, x => x.Value)
                           .ToDictionary(x => x.Key, x => x.First());
        }
    }
}
