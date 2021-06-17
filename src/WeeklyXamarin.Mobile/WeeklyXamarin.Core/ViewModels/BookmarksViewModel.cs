using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Services;
using Xamarin.Essentials.Interfaces;
using System.Diagnostics;
using WeeklyXamarin.Core.Helpers;
using MvvmHelpers;
using WeeklyXamarin.Core.Models;
using MvvmHelpers.Commands;
using System.Linq;

namespace WeeklyXamarin.Core.ViewModels
{
    public class BookmarksViewModel : ArticleListViewModelBase
    {
        private SavedArticleThing _loadedArticles = null;

        public BookmarksViewModel(INavigationService navigation, IAnalytics analytics, IDataStore dataStore, IBrowser browser, IPreferences preferences, IShare share,
            IMessagingService messagingService) : base(navigation, analytics, dataStore, browser, preferences, share, messagingService)
        {
            LoadArticlesCommand = new Command(ExecuteLoadArticlesCommand);
            ToggleBookmarkCommand = new Command<Article>(ExecuteToggleBookmarkArticle);
        }


        void ExecuteLoadArticlesCommand()
        {
            try
            {
                Articles.Clear();

                // get the saved
                CurrentState = ListState.Loading;
                _loadedArticles = dataStore.GetSavedArticles(true);
                Articles.AddRange(_loadedArticles.Articles);
                Title = "Bookmarks";    
                
                CurrentState = Articles.Count == 0 ? ListState.Empty: ListState.None;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                analytics.TrackError(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected override void ExecuteToggleBookmarkArticle(Article article)
        {
            // No change in logic for removed bookmarks
            if (article.IsSaved)
            {
                base.ExecuteToggleBookmarkArticle(article);
                return;
            }

            // Get index of saved article
            int insertIndex = 0;
            bool shouldInsert = false;
            foreach (Article loadedArticle in _loadedArticles.Articles)
            {
                if (loadedArticle.Id == article.Id)
                {
                    shouldInsert = true;
                    break;
                }
                else if (loadedArticle.IsSaved)
                {
                    ++insertIndex;
                }
            }

            // If we aren't inserting, no change in logic
            if (shouldInsert)
            {
                dataStore.BookmarkArticleAtIndex(article, insertIndex);
                article.IsSaved = true;
                BookmarkIcon = Constants.ToolbarIcons.Unbookmark;
            }
            else
            {
                base.ExecuteToggleBookmarkArticle(article);
            }
        }
    }
}
