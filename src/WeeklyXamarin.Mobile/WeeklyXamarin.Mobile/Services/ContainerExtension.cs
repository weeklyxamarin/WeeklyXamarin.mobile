using System;
using WeeklyXamarin.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using WeeklyXamarin.Core.ViewModels;
using System.Net.Http;
using MonkeyCache.FileStore;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;
using Microsoft.Extensions.Logging;

namespace WeeklyXamarin.Mobile.Services
{
    public static class ContainerExtension
    {
        public static IServiceProvider ConfigureServices(Action<ServiceCollection> configure = null)
        {
            var services = new ServiceCollection();

            services.AddSingleton<HttpClient>();
            services.AddSingleton<IDataStore, GithubDataStore>();
            services.AddTransient<EditionsViewModel, EditionsViewModel>();
            services.AddTransient<ArticlesListViewModel, ArticlesListViewModel>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IConnectivity, ConnectivityImplementation>();
            services.AddSingleton<IBrowser, BrowserImplementation>();
            services.AddSingleton(_ => Barrel.Current);

            services.AddLogging(x => x.AddConsole());

            configure?.Invoke(services);

            var serviceProvider = services.BuildServiceProvider();

            serviceProvider.CreateScope();

            return serviceProvider;
        }
    }
}
