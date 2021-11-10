using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Services;
using Xamarin.Essentials.Interfaces;

namespace WeeklyXamarin.Core.ViewModels
{
    public class AuthorsViewModel : ViewModelBase
    {
        private readonly IDataStore dataStore;

        public AuthorsViewModel(INavigationService navigation, IAnalytics analytics, IMessagingService messagingService, IDataStore dataStore, IBrowser browser, IPreferences preferences) : base(navigation, analytics, messagingService, browser, preferences)
        {
            this.dataStore = dataStore;
        }

        public IEnumerable<Author> Authors { get; private set; }

        public override async Task InitializeAsync(object parameter)
        {
            await base.InitializeAsync(parameter);
            Authors = await dataStore.GetAuthorsAsync();
        }
    }
}
