using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Services;

namespace WeeklyXamarin.Blazor.Client.Services
{
    public class WasmAnalytics : IAnalytics
    {
        public Task IsEnabledAsync()
        {
            return Task.CompletedTask;
        }

        public Task SetEnabledAsync(bool enabled)
        {
            return Task.CompletedTask;
        }

        public void TrackError(Exception ex, Dictionary<string, string>? properties = null)
        {
            return;
        }

        public void TrackEvent(string eventName, Dictionary<string, string>? properties = null)
        {
            return;
        }
    }
}
