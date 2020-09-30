using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;
using WeeklyXamarin.Core.Helpers;
using WeeklyXamarin.Core.Services;
using Xamarin.Forms;

namespace WeeklyXamarin.Mobile.Views
{
    public class PageBase : ContentPage { }

    public class PageBase<TViewModel> : PageBase
    {
        private IAnalytics analytics;

        public TViewModel ViewModel { get; set; }

        public PageBase()
        {
            BindingContext = ViewModel = Container.Instance.ServiceProvider.GetRequiredService<TViewModel>();
            analytics = Container.Instance.ServiceProvider.GetRequiredService<IAnalytics>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            analytics.TrackEvent(Constants.Analytics.Events.PageOpened,
                new Dictionary<string, string> 
                {
                    { Constants.Analytics.Properties.PageName, this.GetType().Name } 
                });



        }
    }
}