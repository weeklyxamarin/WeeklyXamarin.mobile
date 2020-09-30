using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Services;

namespace WeeklyXamarin.Mobile.Services
{
    public class AppCenterAnalyticsImplementation : IAnalytics
    {
        public void TrackEvent(string eventName, Dictionary<string, string> properties = null)
            => Microsoft.AppCenter.Analytics.Analytics.TrackEvent(eventName, properties);

        public void TrackError(Exception ex, Dictionary<string, string> properties = null)
            => Microsoft.AppCenter.Crashes.Crashes.TrackError(ex, properties);

        public Task IsEnabledAsync()
            => Microsoft.AppCenter.Analytics.Analytics.IsEnabledAsync();

        public Task SetEnabledAsync(bool enabled)
            => Microsoft.AppCenter.Analytics.Analytics.SetEnabledAsync(enabled);
    }
}
