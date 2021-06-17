using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Helpers;
using WeeklyXamarin.Core.ViewModels;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeeklyXamarin.Mobile.Views
{
    [QueryProperty(nameof(ArticleId), nameof(ArticleId))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArticlePage : PageBase<ArticleViewModel>
    {
        readonly Popup busyPopup;

        public string ArticleId { get; set; }

        public ArticlePage()
        {
            InitializeComponent();
            busyPopup = new Popup()
            {
                Content = new Grid()
                {
                    Style = (Style)Application.Current.Resources["OverlayBackground"],
                    Children = {
                        new ActivityIndicator()
                        {
                            IsRunning = true,
                            HorizontalOptions= LayoutOptions.CenterAndExpand,
                            VerticalOptions = LayoutOptions.CenterAndExpand
                        }
                    }
                },
                IsLightDismissEnabled = false,
                Size = new Size(100, 100)
            };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ViewModel.InitializeAsync(ArticleId);
        }

        private void WebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            Navigation.ShowPopup(busyPopup);
        }

        private async void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            busyPopup.Dismiss(null);

            if (e.Result == WebNavigationResult.Failure)
            {
                await DisplayAlert("Weekly Xamarin", "Article failed to load, please try later.", "OK");
            }
        }
    }
}