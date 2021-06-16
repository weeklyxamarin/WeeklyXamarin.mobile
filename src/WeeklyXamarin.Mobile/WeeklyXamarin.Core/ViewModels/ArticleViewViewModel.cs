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
    public class ArticleViewViewModel : ArticleViewModelBase
    {
        private Article _article;
        protected readonly IBrowser _browser;
        protected readonly IDataStore _dataStore;
        protected readonly IPreferences _preferences;

        public ArticleViewViewModel(INavigationService navigation,
                                    IShare share,
                                    IBrowser browser,
                                    IAnalytics analytics,
                                    IDataStore dataStore,
                                    IPreferences preferences,
                                    IMessagingService messagingService) : base(navigation,
                                                                               share,
                                                                               dataStore,
                                                                               analytics,
                                                                               messagingService)
        {
            _browser = browser;
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
            await _browser.OpenAsync(Article.Url, new BrowserLaunchOptions
            {
                LaunchMode = _preferences.Get(Constants.Preferences.OpenLinksInApp, true) ? BrowserLaunchMode.SystemPreferred : BrowserLaunchMode.External,
                TitleMode = BrowserTitleMode.Show
            });
        }
    }
}
