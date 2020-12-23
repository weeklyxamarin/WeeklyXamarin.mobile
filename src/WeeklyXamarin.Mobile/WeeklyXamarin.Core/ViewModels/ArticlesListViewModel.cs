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
        private ArticlesPageMode _pageMode;
        private bool shouldForceRefresh;

        public ObservableRangeCollection<Article> Articles { get; set; } = new ObservableRangeCollection<Article>();
        public ICommand LoadArticlesCommand { get; set; }
        public ICommand SearchArticlesCommand { get; set; }
        public ICommand OpenArticleCommand { get; set; }
        public ICommand ToggleBookmarkCommand { get; set; }
        public ICommand ShareCommand { get; set; }
        public ICommand NavigateBackCommand { get; set; }
        public string EditionId { get; set; }
        public string SearchText { get; set; }
        public ArticlesPageMode PageMode
        {
            get => _pageMode;
            set => SetProperty(ref _pageMode, value);
        }

        public ArticlesListViewModel(INavigationService navigation, IDataStore dataStore, IBrowser browser, IAnalytics analytics, IPreferences preferences, IShare share) : base(navigation, analytics)
        {
            Title = "Articles";
            LoadArticlesCommand = new AsyncCommand(ExecuteLoadArticlesCommand);
            SearchArticlesCommand = new AsyncCommand(ExecuteSearchArticlesCommand);
            OpenArticleCommand = new AsyncCommand<Article>(OpenArticle);
            ToggleBookmarkCommand = new Command<Article>(ExecuteToggleBookmarkArticle);
            ShareCommand = new AsyncCommand<Article>(ExecuteShareCommand);
            NavigateBackCommand = new AsyncCommand(ExecuteNavigateBackCommand);
            this.dataStore = dataStore;
            this.browser = browser;
            this.preferences = preferences;
            this.share = share;
        }



        private async Task ExecuteNavigateBackCommand()
        {
            await navigation.GoToAsync(Constants.Navigation.Paths.Editions);
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

                if (PageMode == ArticlesPageMode.Bookmarks)
                    Articles.Remove(article);

            }
            else
            {
                dataStore.BookmarkArticle(article);
                article.IsSaved = true;
            }
        }

        // public bool ShouldForceRefresh 
        // { 
        //     get => shouldForceRefresh; 
        //     set => SetProperty(ref shouldForceRefresh, value); 
        // }

        //private bool CanRefresh(object arg)
        //{
        //    return !IsBusy;
        //}

        private async Task OpenArticle(Article article)
        {
            await browser.OpenAsync(article.Url, new BrowserLaunchOptions
            {
                LaunchMode = preferences.Get(Constants.Preferences.OpenLinksInApp, true) ? BrowserLaunchMode.SystemPreferred : BrowserLaunchMode.External,
                TitleMode = BrowserTitleMode.Show
            });
        }

        private Task ExecuteSearchArticlesCommand()
        {
            shouldForceRefresh = false;
            return ExecuteLoadArticlesCommand();
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
                    var articles = dataStore.GetSavedArticles(forceRefresh);
                    Articles.AddRange(articles.Articles);
                    Title = "Bookmarks";
                }
                else if (PageMode == ArticlesPageMode.Search)
                {
                    // don't search for bad things

                    //C# 8 Goodness
                    // if (SearchText is { Length: > 1 })
                    if(SearchText?.Length > 1)
                    {
                        var articlesAsync = dataStore.GetArticleFromSearchAsync(SearchText, forceRefresh);
                        await foreach (Article article in articlesAsync)
                        {
                            Articles.Add(article);
                        }
                    }
                }
                else
                {
                    Title = $"Edition {EditionId}";
                    var edition = await dataStore.GetEditionAsync(EditionId, forceRefresh);
                    Articles.AddRange(edition.Articles);
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