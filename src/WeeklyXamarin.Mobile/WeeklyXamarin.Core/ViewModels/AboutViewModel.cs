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
        private readonly IThemeService theme;

        public List<LinkInfo> AboutLinks { get; set; } = new List<LinkInfo>
        {
            new LinkInfo {Text="Github Repository", Url="https://github.com/weeklyxamarin/WeeklyXamarin.mobile"},
            new LinkInfo {Text="Weekly Xamarin Website", Url="http://weeklyxamarin.com"},
            new LinkInfo {Text="Submit an Article", Url="https://bit.ly/WeeklyXamarinSubmit"},
        };

        public List<Contributor> Contributors { get; set; } = new List<Contributor>
        {
            new Contributor{Name = "Kym Phillpotts", Initials = "KP", ImageUrl="https://avatars.githubusercontent.com/u/1327346", ProfileUrl="https://github.com/kphillpotts"},
            new Contributor{Name="Lachlan Gordon", Initials="LG", ImageUrl="https://avatars.githubusercontent.com/u/29908924", ProfileUrl="https://github.com/lachlanwgordon"},
            new Contributor{Name="Luce Carter", Initials="LC", ImageUrl="https://avatars.githubusercontent.com/u/6980734", ProfileUrl="https://github.com/LuceCarter"},
            new Contributor{Name="Ryan Davis", Initials="RD", ImageUrl="https://avatars.githubusercontent.com/u/7392704", ProfileUrl="https://github.com/rdavisau"},
            new Contributor{Name="David Wahid", Initials="DW", ImageUrl="https://avatars.githubusercontent.com/u/30383473", ProfileUrl="https://github.com/wahid-d"},
            new Contributor{Name="James Montemagno", Initials="JM", ImageUrl="https://avatars.githubusercontent.com/u/1676321", ProfileUrl="https://github.com/jamesmontemagno"},
            new Contributor{Name="Aden Earnshaw", Initials="AE", ImageUrl="https://avatars.githubusercontent.com/u/1441222", ProfileUrl="https://github.com/adenearnshaw"},
            new Contributor{Name="Saamer Mansoor", Initials="SM", ImageUrl="https://avatars.githubusercontent.com/u/8262287", ProfileUrl="https://github.com/saamerm"},
            new Contributor{Name="Jonathan Parker", Initials="JP", ImageUrl="https://avatars.githubusercontent.com/u/152131", ProfileUrl="https://github.com/jonparker"},
            new Contributor{Name="Vijay Anand E G", Initials="VA", ImageUrl="https://avatars.githubusercontent.com/u/81947404", ProfileUrl="https://github.com/egvijayanand"},
        };

        public List<LinkInfo> Libraries { get; set; } = new List<LinkInfo>
        {
            new LinkInfo {Text="Xamarin.Essentials", Url="https://github.com/xamarin/Essentials"},
            new LinkInfo {Text="Monkey Cache", Url="https://github.com/jamesmontemagno/monkey-cache"},
            new LinkInfo {Text="Newtonsoft Json", Url="https://github.com/JamesNK/Newtonsoft.Json"},
            new LinkInfo {Text="Refactored MvvmHelpers", Url="https://github.com/jamesmontemagno/mvvm-helpers"},
            new LinkInfo {Text="Xamarin.Essentials Interfaces", Url="https://github.com/rdavisau/essential-interfaces"},
            new LinkInfo {Text="Lottie Xamarin", Url="https://github.com/Baseflow/LottieXamarin"},
            new LinkInfo {Text="Sharpnado MaterialFrame", Url="https://github.com/roubachof/Sharpnado.MaterialFrame"},
            new LinkInfo {Text="Shiny", Url="https://github.com/shinyorg/shiny"},
            new LinkInfo {Text="Xamarin Community Toolkit", Url="https://github.com/xamarin/XamarinCommunityToolkit"},
            new LinkInfo {Text="Xamarin.Forms", Url="https://github.com/xamarin/Xamarin.Forms"},
            new LinkInfo {Text="Mobile Build Tools", Url="https://github.com/dansiegel/Mobile.BuildTools"},
            new LinkInfo {Text="Microsoft AppCenter", Url="https://www.nuget.org/packages/Microsoft.AppCenter/"},
        };

        public ICommand OpenUrlCommand { get; }
        public ICommand OpenAcknowlegementsCommand { get; }

        public AboutViewModel(INavigationService navigation, IAnalytics analytics, IPreferences preferences, IBrowser browser,
                              IMessagingService messagingService, IThemeService theme) : base(navigation, analytics, messagingService)
        {
            Title = "About";
            OpenUrlCommand = new AsyncCommand<string>(ExecuteOpenUrlCommand);
            OpenAcknowlegementsCommand = new AsyncCommand(ExecuteOpenAcknowledgmentsCommand);

            this.preferences = preferences;
            this.browser = browser;
            this.theme = theme;
        }

        public int Theme
        {
            get => preferences.Get(Constants.Preferences.Theme, 0);
            set
            {
                preferences.Set(Constants.Preferences.Theme, value);
                theme.SetTheme();
            }
        }

        public bool UseSystem
        {
            get => Theme == 0;
            set
            {
                if (!value)
                    return;
                Theme = 0;
            }
        }

        public bool UseLight
        {
            get => Theme == 1;
            set
            {
                if (!value)
                    return;
                Theme = 1;
            }
        }

        public bool UseDark
        {
            get => Theme == 2;
            set
            {
                if (!value)
                    return;
                Theme = 2;
            }
        }

        public bool OpenLinksInBrowser
        {
            get => preferences.Get(Constants.Preferences.OpenLinksInBrowser, true);
            set => preferences.Set(Constants.Preferences.OpenLinksInBrowser, value);
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


        private async Task ExecuteOpenAcknowledgmentsCommand()
        {
            await navigation.GoToAsync(Constants.Navigation.Paths.Acknowlegements);
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