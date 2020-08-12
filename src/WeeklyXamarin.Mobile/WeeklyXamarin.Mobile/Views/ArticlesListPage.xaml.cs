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

namespace WeeklyXamarin.Mobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    [QueryProperty(nameof(EditionId), nameof(EditionId))]
    public partial class ArticlesListPage : PageBase<ArticlesListViewModel>
    {
        private readonly bool showSaved;

        public string EditionId { get; set; }
        public ArticlesListPage()
        {
            InitializeComponent();
        }
        public ArticlesListPage(bool showSaved) : this()
        {
            this.showSaved = showSaved;
        }

        //async void OnItemSelected(object sender, EventArgs args)
        //{
        //    var layout = (BindableObject)sender;
        //    var article = (Article)layout.BindingContext;
        //    await Navigation.PushAsync(new ArticleDetailPage(new ArticleDetailViewModel(article)));
        //}

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.ShowSaved = showSaved;
            ViewModel.EditionId = EditionId;

            if (ViewModel.Articles.Count == 0)
                await ViewModel.LoadArticlesCommand.ExecuteAsync(false);

        }
    }
}