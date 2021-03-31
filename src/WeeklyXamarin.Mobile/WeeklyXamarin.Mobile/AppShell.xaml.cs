using System;
using System.Collections.Generic;
using WeeklyXamarin.Core.Helpers;
using WeeklyXamarin.Mobile.Views;
using Xamarin.Forms;

namespace WeeklyXamarin.Mobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(Constants.Navigation.Paths.Articles,typeof(ArticlesListPage));
            Routing.RegisterRoute(Constants.Navigation.Paths.Acknowlegements,typeof(AcknowledgementsPage));
        }
    }
}
