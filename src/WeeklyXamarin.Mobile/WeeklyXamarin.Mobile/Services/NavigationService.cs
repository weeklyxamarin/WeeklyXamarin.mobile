using System;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Services;
using Xamarin.Forms;

namespace WeeklyXamarin.Mobile.Services
{
    public class NavigationService : INavigationService
    {
        public async Task GoToAsync(string uri)
        {
            await Shell.Current.GoToAsync(uri);
        }

        public async Task GoToAsync(string uri, string parameterKey, string parameterValue)
        {
            await Shell.Current.GoToAsync($"{uri}?{parameterKey}={parameterValue}");
        }
    }
}