using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using WeeklyXamarin.Core.Helpers;
using WeeklyXamarin.Core.Services;
using Xamarin.Essentials.Interfaces;

namespace WeeklyXamarin.Core.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        private readonly IPreferences preferences;
        public AboutViewModel(INavigationService navigation, IAnalytics analytics, IPreferences preferences) : base(navigation, analytics)
        {
            Title = "About";
            //OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://xamarin.com"));
            var thanks = new Acknowledgements();
            Acknowledgements = thanks.Thanks.ToList();
            this.preferences = preferences;
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

        public ICommand OpenWebCommand { get; }
    }
}