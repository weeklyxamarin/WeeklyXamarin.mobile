using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;

namespace WeeklyXamarin.Blazor.Server.Services
{
    public class Connectivity : IConnectivity
    {
        public NetworkAccess NetworkAccess => NetworkAccess.Internet;

        public IEnumerable<ConnectionProfile> ConnectionProfiles => throw new NotImplementedException();

        public event EventHandler<ConnectivityChangedEventArgs> ConnectivityChanged;
    }
}
