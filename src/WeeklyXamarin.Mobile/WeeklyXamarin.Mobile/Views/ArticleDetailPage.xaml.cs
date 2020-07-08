using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Container = WeeklyXamarin.Core.Services.Container;
using WeeklyXamarin.Core.ViewModels;
using WeeklyXamarin.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using WeeklyXamarin.Core.Helpers;
namespace WeeklyXamarin.Mobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    [QueryProperty(nameof(ArticleId), Constants.Navigation.ParameterNames.ArticleId)]
    [QueryProperty(nameof(EditionId), Constants.Navigation.ParameterNames.EditionId)]
    public partial class ArticleDetailPage : ContentPage
    {
        ArticleDetailViewModel viewModel;
        public string ArticleId { get; set; }
        public string EditionId { get; set; }

        public ArticleDetailPage()
        {
            InitializeComponent();

            BindingContext = viewModel = Container.Instance.ServiceProvider.GetRequiredService<ArticleDetailViewModel>();
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.Initialize(EditionId, ArticleId);
        }
    }
}