using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Helpers;
using WeeklyXamarin.Core.ViewModels;
using Xamarin.CommunityToolkit.Extensions;
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

        //TODO: Refactor later!
        void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (query.ContainsKey(nameof(Constants.Navigation.ParameterNames.Category)))
            {
                ViewModel.SearchText = WebUtility.UrlDecode(query[Constants.Navigation.ParameterNames.Category]);
            }
        }

        private async void SelectCategories_Clicked(object sender, EventArgs e)
        {
            object result = await Navigation.ShowPopupAsync(new CategoryPopup(ViewModel.Categories));
            await DisplayAlert("Popup Response", $"{result}", "OK");
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewModel.SelectCategoryCommand.Execute(ViewModel.SearchCategory);
        }
    }
}