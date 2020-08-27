using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Services;
using WeeklyXamarin.Core.Helpers;
using System.Collections.Generic;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials;

namespace WeeklyXamarin.Core.ViewModels
{
    public class ArticlesListViewModel : ViewModelBase
    {
        readonly IDataStore dataStore;
        readonly IBrowser browser;
        readonly IPreferences preferences;
        readonly IShare share;

        public ObservableRangeCollection<Article> Articles { get; set; } = new ObservableRangeCollection<Article>();
        public AsyncCommand<bool> LoadArticlesCommand { get; set; }
        public ICommand OpenArticleCommand { get; set; }
        public ICommand ToggleBookmarkCommand { get; set; }
        public ICommand ShareCommand { get;  set; }
        public string EditionId { get; set;  }
        public bool ShowSaved { get; set; }

        public ArticlesListViewModel(INavigationService navigation, IDataStore dataStore, IBrowser browser, IPreferences preferences, IShare share) : base(navigation)
        {
            Title = "Articles";
            LoadArticlesCommand = new AsyncCommand<bool>(ExecuteLoadArticlesCommand, CanRefresh);
            OpenArticleCommand = new AsyncCommand<Article>(OpenArticle);
            ToggleBookmarkCommand = new Command<Article>(ExecuteToggleBookmarkArticle);
            ShareCommand = new AsyncCommand<Article>(ExecuteShareCommand);
            this.dataStore = dataStore;
            this.browser = browser;
            this.preferences = preferences;
            this.share = share;
        }

        private async Task ExecuteShareCommand(Article article)
        {

            await share.RequestAsync(new ShareTextRequest
            {
                Uri = article.Url,
                Title = article.Title
            });
        }

        private void ExecuteToggleBookmarkArticle(Article article)
        {
            if (article.IsSaved)
            {
                dataStore.UnbookmarkArticle(article);
                article.IsSaved = false;

                if (ShowSaved)
                    Articles.Remove(article);

            }
            else
            {
                dataStore.BookmarkArticle(article);
                article.IsSaved = true;
            }
        }

        private bool CanRefresh(object arg)
        {
            return !IsBusy;
        }

        private async Task OpenArticle(Article article)
        { 
            await browser.OpenAsync(article.Url, new BrowserLaunchOptions
            {
                LaunchMode = preferences.Get(Constants.Preferences.OpenLinksInApp, true) ? BrowserLaunchMode.SystemPreferred : BrowserLaunchMode.External,
                TitleMode = BrowserTitleMode.Show
            });
        }

        async Task ExecuteLoadArticlesCommand(bool forceRefresh = false)
        {
            IsBusy = true;

            try
            {
                Articles.Clear();

                if (ShowSaved)
                {
                    // get the saved
                    var articles = dataStore.GetSavedArticles(forceRefresh);
                    Articles.AddRange(articles.Articles);
                }
                else
                {
                    var edition = await dataStore.GetEditionAsync(EditionId, forceRefresh);
                    Articles.AddRange(edition.Articles);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}