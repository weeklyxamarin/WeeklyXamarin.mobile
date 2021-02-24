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

namespace WeeklyXamarin.Core.ViewModels
{
    public class BookmarksViewModel : ArticleListViewModelBase
    {

        public BookmarksViewModel(INavigationService navigation, IAnalytics analytics, IDataStore dataStore, IBrowser browser, IPreferences preferences, IShare share) : base(navigation, analytics, dataStore, browser, preferences, share)
        {
            LoadArticlesCommand = new Command(ExecuteLoadArticlesCommand);
        }

        void ExecuteLoadArticlesCommand()
        {
            try
            {
                Articles.Clear();

                // get the saved
                CurrentState = ListState.Loading;
                var articles = dataStore.GetSavedArticles(true);
                Articles.AddRange(articles.Articles);
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


    }
}
