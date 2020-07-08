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
            var fullUrl = new StringBuilder();
            fullUrl.Append(uri);
            fullUrl.Append("?");
            fullUrl.Append(string.Join("&", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}")));

            await Shell.Current.GoToAsync(fullUrl.ToString());
        }
    }
}