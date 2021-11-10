using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using WeeklyXamarin.Core.Helpers;
using WeeklyXamarin.Core.Services;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;

namespace WeeklyXamarin.Core.ViewModels
{
    public class ViewModelBase : MvvmHelpers.BaseViewModel
    {
        public ICommand CloseViewCommand { get; set; }

        public ICommand OpenUrlCommand { get; }

        protected INavigationService navigation;
        protected IAnalytics analytics;
        protected IMessagingService messagingService;
        private IBrowser browser;
        private IPreferences preferences;

        public ViewModelBase(INavigationService navigation, IAnalytics analytics, IMessagingService messagingService, IBrowser browser, IPreferences preferences)
        {
            CloseViewCommand = new AsyncCommand(ExecuteCloseViewCommand);
            OpenUrlCommand = new AsyncCommand<string>(ExecuteOpenUrlCommand);
            this.navigation = navigation;
            this.analytics = analytics;
            this.messagingService = messagingService;
            this.browser = browser;
            this.preferences = preferences;
        }

        public virtual Task InitializeAsync(object parameter)
        {
            return Task.FromResult(0);
        }

        private async Task ExecuteCloseViewCommand()
        {
            await navigation.GoBackAsync();
        }

        private async Task ExecuteOpenUrlCommand(string url)
        {
            await browser.OpenAsync(url, new BrowserLaunchOptions
            {
                LaunchMode = preferences.Get(Constants.Preferences.OpenLinksInBrowser, true) ? BrowserLaunchMode.SystemPreferred : BrowserLaunchMode.External,
                TitleMode = BrowserTitleMode.Show
            });
        }
    }
}
