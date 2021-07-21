using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Services;

namespace WeeklyXamarin.Blazor.Client.Services
{
    public class WasmNavigationService : INavigationService
    {
        public Task GoBackAsync()
        {
            throw new NotImplementedException();
        }

        public Task GoToAsync(string uri)
        {
            throw new NotImplementedException();
        }

        public Task GoToAsync(string uri, string parameterKey, string parameterValue)
        {
            throw new NotImplementedException();
        }

        public Task GoToAsync(string uri, Dictionary<string, string> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
