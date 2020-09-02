using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WeeklyXamarin.Core.Services;
using WeeklyXamarin.Mobile.Views;
using Microsoft.Extensions.DependencyInjection;
using MonkeyCache.FileStore;
using WeeklyXamarin.Mobile.Services;

namespace WeeklyXamarin.Mobile
{
    public partial class App : Application
    {
        public App() : this(null) { }

        public App(Action<ServiceCollection> configure)
        {
            InitializeComponent();

            Device.SetFlags(new[] { "SwipeView_Experimental",
            "Shell_UWP_Experimental"});
            Barrel.ApplicationId = "WeeklyXamarin";
           
            Container.Instance.ServiceProvider 
                = ContainerExtension.ConfigureServices(configure);


            //MainPage = new ContentPage
            //{
            //    Content = new StackLayout
            //    {
            //        Children =
            //        {
            //            new Label
            //            {
            //                Text = "HELLLOOOOOO",
            //                VerticalOptions = LayoutOptions.CenterAndExpand,
            //                HorizontalOptions = LayoutOptions.Center
            //            }
            //        }
            //    }
            //};

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
