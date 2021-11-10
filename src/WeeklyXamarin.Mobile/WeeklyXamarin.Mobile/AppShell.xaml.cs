﻿using System;
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
            Routing.RegisterRoute(Constants.Navigation.Paths.ArticleView, typeof(ArticlePage));
            Routing.RegisterRoute(Constants.Navigation.Paths.Author, typeof(AuthorPage));
        }
    }
}
