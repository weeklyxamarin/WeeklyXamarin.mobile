using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using WeeklyXamarin.Core.Helpers;
using WeeklyXamarin.Core.Services;
using Xamarin.Essentials.Interfaces;
using MvvmHelpers.Commands;
using System.Threading.Tasks;
using Xamarin.Essentials;
using WeeklyXamarin.Core.Models;

namespace WeeklyXamarin.Core.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        private readonly IPreferences preferences;
        private readonly IBrowser browser;
        public List<LinkInfo> AboutLinks { get; set; } = new List<LinkInfo>
        {
            new LinkInfo {Text="Github Repository", Url="https://github.com/weeklyxamarin/WeeklyXamarin.mobile"},
            new LinkInfo {Text="Weekly Xamarin Website", Url="http://weeklyxamarin.com"}
        };

        public List<LinkInfo> Libraries { get; set; } = new List<LinkInfo>
        {
            new LinkInfo {Text="Xamarin.Essentials", Url="https://github.com/xamarin/Essentials"},
            new LinkInfo {Text="LottieXamarin", Url="https://github.com/Baseflow/LottieXamarin"}
        };


        

        public ICommand OpenUrlCommand { get; }

        public AboutViewModel(INavigationService navigation, IAnalytics analytics, IPreferences preferences, IBrowser browser) : base(navigation, analytics)
        {
            Title = "About";
            OpenUrlCommand = new AsyncCommand<string>(ExecuteOpenUrlCommand);
            var thanks = new Acknowledgements();
            Acknowledgements = thanks.Thanks.ToList();
            this.preferences = preferences;
            this.browser = browser;
        }

        public bool OpenLinksInApp
        {
            get => preferences.Get(Constants.Preferences.OpenLinksInApp, true);
            set => preferences.Set(Constants.Preferences.OpenLinksInApp, value);
        }

        public bool Analytics
        {
            get => preferences.Get(Constants.Preferences.Analytics, true);
            set
            {
                _ = analytics.SetEnabledAsync(value);
                preferences.Set(Constants.Preferences.Analytics, value);
            }
        }

        public List<Acknowledgement> Acknowledgements { get; set; }


        private async Task ExecuteOpenUrlCommand(string url)
        { 
            await browser.OpenAsync(url, new BrowserLaunchOptions
            {
                LaunchMode = preferences.Get(Constants.Preferences.OpenLinksInApp, true) ? BrowserLaunchMode.SystemPreferred : BrowserLaunchMode.External,
                TitleMode = BrowserTitleMode.Show
            });
        }
    }
}