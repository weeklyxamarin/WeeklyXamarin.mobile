using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeeklyXamarin.Core.Helpers;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Services;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;

namespace WeeklyXamarin.Core.ViewModels
{
    public abstract class ArticleListViewModelBase : ViewModelBase
    {
        protected readonly IDataStore dataStore;
        protected readonly IBrowser browser;
        protected readonly IShare share;
        protected readonly IPreferences preferences;
        private ListState currentState;

        public ArticleListViewModelBase(INavigationService navigation, 
            IAnalytics analytics,
            IDataStore dataStore,
            IBrowser browser,
            IPreferences preferences,
            IShare share) : base(navigation, analytics)
        {
            OpenArticleCommand = new AsyncCommand<Article>(OpenArticle);
            ShareCommand = new AsyncCommand<Article>(ExecuteShareCommand);
            ToggleBookmarkCommand = new Command<Article>(ExecuteToggleBookmarkArticle);

            this.dataStore = dataStore;
            this.browser = browser;
            this.preferences = preferences;
            this.share = share;
        }

        public ICommand LoadArticlesCommand { get; set; }
        public ICommand OpenArticleCommand { get; set; }
        public ICommand ToggleBookmarkCommand { get; set; }
        public ICommand ShareCommand { get; set; }
        public ObservableRangeCollection<Article> Articles { get; set; } = new ObservableRangeCollection<Article>();
        public ListState CurrentState 
        { 
            get => currentState;
            set => SetProperty(ref currentState, value);
        }

        private async Task ExecuteShareCommand(Article article)
        {
            await share.RequestAsync(new ShareTextRequest
            {
                Uri = article.Url,
                Title = article.Title
            });
        }

        protected virtual void ExecuteToggleBookmarkArticle(Article article)
        {
            if (article.IsSaved)
            {
                dataStore.UnbookmarkArticle(article);
                article.IsSaved = false;
            }
            else
            {
                dataStore.BookmarkArticle(article);
                article.IsSaved = true;
            }
        }

        private async Task OpenArticle(Article article)
        {
            await browser.OpenAsync(article.Url, new BrowserLaunchOptions
            {
                LaunchMode = preferences.Get(Constants.Preferences.OpenLinksInApp, true) ? BrowserLaunchMode.SystemPreferred : BrowserLaunchMode.External,
                TitleMode = BrowserTitleMode.Show
            });
        }

    }
}
