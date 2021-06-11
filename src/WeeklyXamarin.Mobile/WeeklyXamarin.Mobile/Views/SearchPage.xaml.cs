using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WeeklyXamarin.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeeklyXamarin.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : PageBase<SearchViewModel>, IQueryAttributable
    {
        // Name of the query string id
        const string Category = "";

        public SearchPage()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            // Clearing the search text while navigating away
            ViewModel.SearchText = string.Empty;
            base.OnDisappearing();
        }

        void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (query.ContainsKey(nameof(Category)))
            {
                ViewModel.SearchText = WebUtility.UrlDecode(query[nameof(Category)]);
            }
        }
    }
}