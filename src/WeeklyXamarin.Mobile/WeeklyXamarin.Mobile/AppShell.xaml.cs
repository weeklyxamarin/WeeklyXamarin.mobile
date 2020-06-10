using System;
using System.Collections.Generic;
using WeeklyXamarin.Mobile.Views;
using Xamarin.Forms;

namespace WeeklyXamarin.Mobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("articles",typeof(ArticlesListPage));
            Routing.RegisterRoute("articledetail", typeof(ArticleDetailPage));
        }
    }
}
