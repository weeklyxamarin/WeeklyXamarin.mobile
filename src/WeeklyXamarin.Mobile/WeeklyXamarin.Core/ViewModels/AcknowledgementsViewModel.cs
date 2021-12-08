using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Services;
using Xamarin.Essentials.Interfaces;

namespace WeeklyXamarin.Core.ViewModels
{
    public class AcknowledgementsViewModel : ViewModelBase
    {
        private IDataStore dataStore;

        public List<Acknowledgement> Acknowledgements { get; set; }

        public AcknowledgementsViewModel(INavigationService navigation, IAnalytics analytics,
            IMessagingService messagingService, IBrowser browser, IPreferences preferences, IDataStore dataStore) : base(navigation, analytics, messagingService, browser, preferences)
        {
            this.dataStore = dataStore;
        }

        public override async Task InitializeAsync(object parameter)
        {
            await base.InitializeAsync(parameter);
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var acknowledgements = await dataStore.GetAcknowledgementsAsync();
            Acknowledgements = acknowledgements.ToList();
        }
    }
}
