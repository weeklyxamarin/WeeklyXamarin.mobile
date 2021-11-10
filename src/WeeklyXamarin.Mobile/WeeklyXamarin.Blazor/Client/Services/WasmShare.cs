using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;

namespace WeeklyXamarin.Blazor.Client.Services
{
    public class WasmShare : IShare
    {
        public Task RequestAsync(string text)
        {
            throw new NotImplementedException();
        }

        public Task RequestAsync(string text, string title)
        {
            throw new NotImplementedException();
        }

        public Task RequestAsync(ShareTextRequest request)
        {
            throw new NotImplementedException();
        }

        public Task RequestAsync(ShareFileRequest request)
        {
            throw new NotImplementedException();
        }

        public Task RequestAsync(ShareMultipleFilesRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
