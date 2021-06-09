using System;
using System.Collections.Generic;
using System.Text;
using WeeklyXamarin.Core.Helpers;
using WeeklyXamarin.Core.Services;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace WeeklyXamarin.Mobile.Services
{
    public class ThemeService : IThemeService
    {
        IPreferences preferences;
        IStatusBarService statusBar;
        public ThemeService(IPreferences preferences, IStatusBarService statusBar)
        {
            this.preferences = preferences;
            this.statusBar = statusBar;
        }

        public int Theme
        {
            get => preferences.Get(Constants.Preferences.Theme, 0);
            set => preferences.Set(Constants.Preferences.Theme, value);
        }

        public void SetTheme()
        {
            switch (Theme)
            {
                //default
                case 0:
                    Application.Current.UserAppTheme = OSAppTheme.Unspecified;
                    break;
                //light
                case 1:
                    Application.Current.UserAppTheme = OSAppTheme.Light;
                    break;
                //dark
                case 2:
                    Application.Current.UserAppTheme = OSAppTheme.Dark;
                    break;
            }

            var nav = Application.Current.MainPage as Xamarin.Forms.NavigationPage;

            if (Application.Current.RequestedTheme == OSAppTheme.Dark)
            {
                var c = (Color)Application.Current.Resources["NavigationPrimaryDark"];
                statusBar?.SetStatusBarColor(c, false);
                if (nav != null)
                {
                    nav.BarBackgroundColor = c;
                    nav.BarTextColor = Color.White;
                }
            }
            else
            {
                var c = (Color)Application.Current.Resources["NavigationPrimaryLight"];
                statusBar?.SetStatusBarColor(c, false);
                if (nav != null)
                {
                    nav.BarBackgroundColor = c;
                    nav.BarTextColor = Color.White;
                }
            }
        }
    }
}
