using Microsoft.Extensions.DependencyInjection;
using Shiny.Jobs;
using System;
using Shiny;
using System.Collections.Generic;
using System.Text;
using WeeklyXamarin.Core.Services;
using System.Net.Http;
using MonkeyCache.FileStore;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;
using Microsoft.Extensions.Logging;
using Shiny.Notifications;
using WeeklyXamarin.Mobile.Services;

namespace WeeklyXamarin.Mobile.Services
{
    public class WeeklyXamarinStartup : Shiny.ShinyStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            var job = new JobInfo(typeof(FetchEditionJob));

            services.RegisterJob(job);
            services.UseNotifications(true);
            services.AddSingleton<HttpClient>(_ => new HttpClient());
            services.AddSingleton<IDataStore, GithubDataStore>();
            services.AddSingleton<IConnectivity, ConnectivityImplementation>();
            services.AddSingleton<IPreferences, PreferencesImplementation>();
            services.AddSingleton<IAnalytics, AppCenterAnalyticsImplementation>();
            Barrel.ApplicationId = "WeeklyXamarin";
            services.AddSingleton(_ => Barrel.Current);
            services.AddLogging(x => x.AddConsole());
        }
    }
}
