using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Helpers;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Services;

namespace WeeklyXamarin.Core.ViewModels
{
    public class AuthorViewModel : ViewModelBase
    {
        private readonly IDataStore dataStore;
        public ListState CurrentState { get; set; }

        public AuthorViewModel(
            INavigationService navigation, 
            IAnalytics analytics, 
            IMessagingService messagingService, 
            IDataStore dataStore) : base(navigation, analytics, messagingService)
        {
            this.dataStore = dataStore;
        }

        public Author Author { get; private set; }

        public override async Task InitializeAsync(object parameter)
        {
            await base.InitializeAsync(parameter);
            var authorId = parameter as string;
            Author = await dataStore.GetAuthorAsync(authorId);
        }
    }
}
