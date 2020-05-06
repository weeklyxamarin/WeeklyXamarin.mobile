using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using WeeklyXamarin.Mobile.Models;
using WeeklyXamarin.Mobile.Views;
using WeeklyXamarin.Mobile.ViewModels;

namespace WeeklyXamarin.Mobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ArticlesListPage : ContentPage
    {
        ArticlesListViewModel viewModel;

        public ArticlesListPage(string editionId)
        {
            InitializeComponent();

            BindingContext = viewModel = new ArticlesListViewModel(editionId);
        }

        async void OnItemSelected(object sender, EventArgs args)
        {
            var layout = (BindableObject)sender;
            var article = (Article)layout.BindingContext;
            await Navigation.PushAsync(new ArticleDetailPage(new ArticleDetailViewModel(article)));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Articles.Count == 0)
                viewModel.IsBusy = true;
        }
    }
}