using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WeeklyXamarin.Mobile.Views;
using WeeklyXamarin.Core.ViewModels;
using WeeklyXamarin.Core.Models;
using Container = WeeklyXamarin.Core.Services.Container;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;
using Lottie.Forms;
using WeeklyXamarin.Core.Helpers;

namespace WeeklyXamarin.Mobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    [QueryProperty(nameof(EditionId), nameof(EditionId))]
    public partial class ArticlesListPage : PageBase<ArticlesListViewModel>
    {
        private ArticlesPageMode PageMode = ArticlesPageMode.Edition;

        public string EditionId { get; set; }
        public ArticlesListPage()
        {
            InitializeComponent();
        }
        public ArticlesListPage(ArticlesPageMode pageMode) : this()
        {
            PageMode = pageMode;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.PageMode = PageMode;
            ViewModel.EditionId = EditionId;

            await ViewModel.LoadArticlesCommand.ExecuteAsync(false);
        }
    }
}