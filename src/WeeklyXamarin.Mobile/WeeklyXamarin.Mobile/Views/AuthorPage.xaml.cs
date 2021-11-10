using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyXamarin.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeeklyXamarin.Mobile.Views
{
    [QueryProperty(nameof(AuthorId), nameof(AuthorId))]

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthorPage : PageBase<AuthorViewModel>
    {
        public string AuthorId { get; set; }
        
        public AuthorPage()
        {
            InitializeComponent();
        }

        private bool firstLoad = true;

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (firstLoad)
            {
                await ViewModel.InitializeAsync(AuthorId);
                await ViewModel.LoadArticles();
                firstLoad = false;
            }
        }
    }
}