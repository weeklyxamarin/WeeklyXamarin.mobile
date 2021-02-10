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
        private ArticlesPageMode _pageMode;
        private bool shouldForceRefresh;

        
        public ICommand NavigateBackCommand { get; set; }
        public string EditionId { get; set; }




        public ArticlesPageMode PageMode
        {
            get => _pageMode;
            set => SetProperty(ref _pageMode, value);
        }

        public ArticlesListViewModel(INavigationService navigation,
            IAnalytics analytics,
            IDataStore dataStore, 
            IBrowser browser, 
            IPreferences preferences, 
            IShare share) : base(navigation, analytics, dataStore, browser, preferences, share)
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

        private void ExecuteToggleBookmarkArticle(Article article)
        {
            if (article.IsSaved)
            {
                dataStore.UnbookmarkArticle(article);
                article.IsSaved = false;

                if (PageMode == ArticlesPageMode.Bookmarks)
                {
                    Articles.Remove(article);
                    if (Articles.Count == 0) CurrentState = ListState.Error;
                }

            }
            else
            {
                dataStore.BookmarkArticle(article);
                article.IsSaved = true;
            }
        }


        async Task ExecuteLoadArticlesCommand()
        {
            var forceRefresh = shouldForceRefresh;

            try
            {
                Articles.Clear();


                if (PageMode == ArticlesPageMode.Bookmarks)
                {
                    // get the saved
                    CurrentState = ListState.Loading;
                    var articles = dataStore.GetSavedArticles(forceRefresh);
                    Articles.AddRange(articles.Articles);
                    Title = "Bookmarks";
                    CurrentState = Articles.Count == 0 ? ListState.Error : ListState.Success;
                }
                else
                {
                    CurrentState = ListState.Loading; 
                    Title = $"Edition {EditionId}";
                    var edition = await dataStore.GetEditionAsync(EditionId, forceRefresh);
                    Articles.AddRange(edition.Articles);
                    CurrentState = ListState.Success;

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                analytics.TrackError(ex, new Dictionary<string, string> {
                    { Constants.Analytics.Properties.EditionId, EditionId },
                    { nameof(PageMode), PageMode.ToString() } });

            }
            finally
            {
                IsBusy = false;
                shouldForceRefresh = true;
            }
        }
    }
}