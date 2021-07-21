using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;

namespace WeeklyXamarin.Blazor.Client.Services
{
    public class WasmConnectivity : IConnectivity
    {
        public NetworkAccess NetworkAccess => NetworkAccess.Internet;

        public IEnumerable<ConnectionProfile> ConnectionProfiles => throw new NotImplementedException();

        public event EventHandler<ConnectivityChangedEventArgs> ConnectivityChanged;
    }
}
