using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WeeklyXamarin.Core.Services;
using Xamarin.Forms;

namespace WeeklyXamarin.Mobile.Views
{
    public class PageBase : ContentPage { }

    public class PageBase<TViewModel> : PageBase
    {
        public TViewModel ViewModel { get; set; }

        public PageBase()
        {
            BindingContext = ViewModel = Container.Instance.ServiceProvider.GetRequiredService<TViewModel>();
        }
    }
}