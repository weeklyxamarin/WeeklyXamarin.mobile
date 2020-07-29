using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task GoToAsync(string uri, Dictionary<string, string> parameters)
        {
            var fullUrl = BuildUri(uri, parameters);
            await Shell.Current.GoToAsync(fullUrl);
        }

        public async Task GoBackAsync()
        {
            // TODO: Work out why GoToAsync("..") throws an exception with modal pages
            if (Shell.Current.Navigation.ModalStack.Count > 0)
                await Shell.Current.Navigation.PopModalAsync();
            else
                await Shell.Current.Navigation.PopAsync();
        }

        private string BuildUri(string uri, Dictionary<string, string> parameters)
        {
            var fullUrl = new StringBuilder();
            fullUrl.Append(uri);
            fullUrl.Append("?");
            fullUrl.Append(string.Join("&", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}")));
            return fullUrl.ToString();
        }
    }
}