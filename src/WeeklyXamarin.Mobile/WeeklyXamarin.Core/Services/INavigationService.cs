using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WeeklyXamarin.Core.Services
{
    public interface INavigationService
    {
        Task GoToAsync(string uri);
        Task GoToAsync(string uri, string parameterKey, string parameterValue);
        Task GoToAsync(string uri, Dictionary<string, string> parameters);
        Task GoBackAsync();
    }

    
}
