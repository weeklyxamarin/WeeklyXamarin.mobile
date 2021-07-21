using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;

namespace WeeklyXamarin.Blazor.Client.Services
{
    public class WasmBrowser : IBrowser
    {
        public Task OpenAsync(string uri)
        {
            throw new NotImplementedException();
        }

        public Task OpenAsync(string uri, BrowserLaunchMode launchMode)
        {
            throw new NotImplementedException();
        }

        public Task OpenAsync(string uri, BrowserLaunchOptions options)
        {
            throw new NotImplementedException();
        }

        public Task OpenAsync(Uri uri)
        {
            throw new NotImplementedException();
        }

        public Task OpenAsync(Uri uri, BrowserLaunchMode launchMode)
        {
            throw new NotImplementedException();
        }

        public Task<bool> OpenAsync(Uri uri, BrowserLaunchOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
