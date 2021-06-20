using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Helpers;
using WeeklyXamarin.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeeklyXamarin.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : PageBase<SearchViewModel>, IQueryAttributable
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ViewModel.InitializeAsync();
        }

        protected override void OnDisappearing()
        {
            // Clearing the search text while navigating away
            ViewModel.SearchText = string.Empty;
            ViewModel.SearchByCategory = false;
            base.OnDisappearing();
        }

        void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (query.ContainsKey(nameof(Constants.Navigation.ParameterNames.Category)))
            {
                ViewModel.SearchText = WebUtility.UrlDecode(query[Constants.Navigation.ParameterNames.Category]);
                ViewModel.SearchByCategory = true;
            }
        }
    }
}