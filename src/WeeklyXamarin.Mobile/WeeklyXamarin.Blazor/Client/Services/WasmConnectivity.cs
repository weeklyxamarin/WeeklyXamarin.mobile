using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;

namespace WeeklyXamarin.Blazor.Client.Services
{
    public class WasmConnectivity : IConnectivity, IDisposable
    {
        private IJSRuntime jsRuntime;

        public WasmConnectivity(IJSRuntime jSRuntime)
        {
            this.jsRuntime = jSRuntime;
            _ = jsRuntime.InvokeVoidAsync("Network.Initialize", DotNetObjectReference.Create(this));
        }

        [JSInvokable("Network.StatusChanged")]
        public void OnStatusChanged(bool isOnline)
        {
            if (IsOnline != isOnline)
            {
                IsOnline = isOnline;
            }
        }

        public void Dispose()
        {
            _ = jsRuntime.InvokeVoidAsync("Network.Dispose");
        }

        public NetworkAccess NetworkAccess => IsOnline ? NetworkAccess.Internet : NetworkAccess.None;

        public IEnumerable<ConnectionProfile> ConnectionProfiles => throw new NotImplementedException();

        public bool IsOnline { get; private set; }

        public event EventHandler<ConnectivityChangedEventArgs> ConnectivityChanged;
    }
}
