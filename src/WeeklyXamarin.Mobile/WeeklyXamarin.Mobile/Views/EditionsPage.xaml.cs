using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WeeklyXamarin.Mobile.Views;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.ViewModels;
using Container = WeeklyXamarin.Core.Services.Container;
using Microsoft.Extensions.DependencyInjection;

namespace WeeklyXamarin.Mobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class EditionsPage : ContentPage
    {
        EditionsViewModel viewModel;

        public EditionsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = Container.Instance.ServiceProvider.GetRequiredService<EditionsViewModel>();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Editions.Count == 0)
                viewModel.IsBusy = true;
        }
    }
}