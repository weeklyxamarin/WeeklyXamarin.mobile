﻿using System;
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

        protected override void ExecuteToggleBookmarkArticle(Article article)
        {
            base.ExecuteToggleBookmarkArticle(article);

            if (!article.IsSaved)
                Articles.Remove(article);

            if (!Articles.Any())
                CurrentState = ListState.Empty;
        }
    }
}
