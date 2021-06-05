using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WeeklyXamarin.Core.Services;
using WeeklyXamarin.Mobile.Views;
using Microsoft.Extensions.DependencyInjection;
using MonkeyCache.FileStore;
using WeeklyXamarin.Mobile.Services;
using Microsoft.AppCenter;
using WeeklyXamarin.Mobile.Helpers;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials.Interfaces;
using WeeklyXamarin.Core.Helpers;
using Xamarin.Essentials;

namespace WeeklyXamarin.Mobile
{
    public partial class App : Application
    {
        private IPreferences preferences;
        private IThemeService theme;

        public App() : this(null) { }

        public App(Action<ServiceCollection> configure)
        {
            InitializeComponent();

            Xamarin.Forms.Device.SetFlags(new[] { "SwipeView_Experimental",
            "Shell_UWP_Experimental"});
            Barrel.ApplicationId = "WeeklyXamarin";
           
            Container.Instance.ServiceProvider 
                = ContainerExtension.ConfigureServices(configure);

            preferences = Container.Instance.ServiceProvider.GetRequiredService<IPreferences>();
            theme = Container.Instance.ServiceProvider.GetRequiredService<IThemeService>();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            if (preferences.Get(Constants.Preferences.Analytics, true))
            {
                AppCenter.Start($"android={Secrets.AppCenterDroid};"
                                + $"ios={Secrets.AppCenteriOS};",
                                typeof(Analytics), typeof(Crashes));
            }

            OnResume();
        }

        protected override void OnSleep()
        {
            theme.SetTheme();

            RequestedThemeChanged -= App_RequestedThemeChanged;
        }

        protected override void OnResume()
        {
            theme.SetTheme();

            RequestedThemeChanged += App_RequestedThemeChanged;
        }

        private void App_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                theme.SetTheme();
            });
        }
    }
}
