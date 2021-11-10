using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
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
    public class ArticleViewModel : ArticleViewModelBase
    {
        private Article _article;
        protected readonly IBrowser browser;
        protected readonly IDataStore _dataStore;
        protected readonly IPreferences _preferences;

        public ArticleViewModel(INavigationService navigation,
                                    IShare share,
                                    IBrowser browser,
                                    IAnalytics analytics,
                                    IDataStore dataStore,
                                    IPreferences preferences,
                                    IMessagingService messagingService) : base(navigation,
                                                                               share,
                                                                               dataStore,
                                                                               analytics,
                                                                               messagingService,
                                                                               browser, preferences)
        {
            this.browser = browser;
            _dataStore = dataStore;
            _preferences = preferences;
            OpenArticleCommand = new AsyncCommand(OpenArticle);
        }

        public Article Article
        {
            get => _article;
            set => SetProperty(ref _article, value);
        }

        public IAsyncCommand OpenArticleCommand { get; }

        public override async Task InitializeAsync(object parameter)
        {
            await base.InitializeAsync(parameter);
            Article = await _dataStore.GetArticleAsync(parameter.ToString().Substring(0, parameter.ToString().IndexOf("-")),
                                                       parameter.ToString());

            BookmarkIcon = Article.IsSaved ? Constants.ToolbarIcons.Unbookmark : Constants.ToolbarIcons.Bookmark;
        }

        private async Task OpenArticle()
        {
            await browser.OpenAsync(Article.Url, new BrowserLaunchOptions
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Show
                });
        }
    }
}
