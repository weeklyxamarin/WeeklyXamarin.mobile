using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using WeeklyXamarin.Mobile.Models;
using WeeklyXamarin.Mobile.ViewModels;

namespace WeeklyXamarin.Mobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ArticleDetailPage : ContentPage
    {
        ArticleDetailViewModel viewModel;

        public ArticleDetailPage(ArticleDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public ArticleDetailPage()
        {
            InitializeComponent();

            //var item = new Item
            //{
            //    Text = "Item 1",
            //    Description = "This is an item description."
            //};

            //viewModel = new ArticleDetailViewModel(item);
            //BindingContext = viewModel;
        }
    }
}