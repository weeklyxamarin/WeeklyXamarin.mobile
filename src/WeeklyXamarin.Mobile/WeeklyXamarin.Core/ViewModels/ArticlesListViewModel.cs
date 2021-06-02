using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Services;
using WeeklyXamarin.Core.Helpers;
using System.Collections.Generic;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials;
using System.Linq;

namespace WeeklyXamarin.Core.ViewModels
{
    public class ArticlesListViewModel : ArticleListViewModelBase
    {
        private bool shouldForceRefresh;
        public ICommand NavigateBackCommand { get; set; }
        public string EditionId { get; set; }

        public ArticlesListViewModel(INavigationService navigation,
            IAnalytics analytics,
            IDataStore dataStore, 
            IBrowser browser, 
            IPreferences preferences, 
            IShare share,
            IMessagingService messagingService) : base(navigation, analytics, dataStore, browser, preferences, share, messagingService)
        {
            Title = "Articles";
            LoadArticlesCommand = new AsyncCommand(ExecuteLoadArticlesCommand);
            ToggleBookmarkCommand = new Command<Article>(ExecuteToggleBookmarkArticle);
            NavigateBackCommand = new AsyncCommand(ExecuteNavigateBackCommand);
           
        }

        

        private async Task ExecuteNavigateBackCommand()
        {
            await navigation.GoToAsync(Constants.Navigation.Paths.Editions);
        }

        async Task ExecuteLoadArticlesCommand()
        {
            try
            {
                Articles.Clear();
                CurrentState = ListState.Loading; 
                Title = $"Edition {EditionId}";
                var edition = await dataStore.GetEditionAsync(EditionId);
                CurrentState = ListState.Success;
                Articles.AddRange(edition.Articles);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                analytics.TrackError(ex, new Dictionary<string, string> {
                    { Constants.Analytics.Properties.EditionId, EditionId }});
            }
            finally
            {
                IsBusy = false;
                shouldForceRefresh = true;
            }
        }
    }
}