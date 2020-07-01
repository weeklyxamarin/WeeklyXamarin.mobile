using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WeeklyXamarin.Core.Services;
using WeeklyXamarin.Mobile.Views;
using Microsoft.Extensions.DependencyInjection;
using WeeklyXamarin.Core.ViewModels;
using WeeklyXamarin.Mobile.Services;
using System.Net.Http;
using MonkeyCache.FileStore;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;

namespace WeeklyXamarin.Mobile
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider {get;}
        public App()
        {
            InitializeComponent();

            // Register our MonkeyBarrel
            Barrel.ApplicationId = "WeeklyXamarin";

            //DependencyService.Register<MockDataStore>();
            var services = new ServiceCollection();
            services.AddSingleton<HttpClient>();
            //services.AddSingleton<IDataStore, MockDataStore>();
            services.AddSingleton<IDataStore, GithubDataStore>();
            services.AddTransient<EditionsViewModel, EditionsViewModel>();
            services.AddTransient<ArticlesListViewModel, ArticlesListViewModel>();
            services.AddTransient<ArticleDetailViewModel, ArticleDetailViewModel>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IConnectivity, ConnectivityImplementation>();

            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.CreateScope();

            Container.Instance.ServiceProvider = serviceProvider;
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
