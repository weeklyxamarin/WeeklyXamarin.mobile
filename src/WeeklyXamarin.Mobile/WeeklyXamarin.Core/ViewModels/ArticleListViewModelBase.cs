using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public abstract class ArticleListViewModelBase : ArticleViewModelBase
    {
        protected readonly IBrowser browser;
        protected readonly IPreferences preferences;
        private ListState currentState;
        private ObservableRangeCollection<Article> articles = new ObservableRangeCollection<Article>();

        public ArticleListViewModelBase(INavigationService navigation,
            IAnalytics analytics,
            IDataStore dataStore,
            IBrowser browser,
            IPreferences preferences,
            IShare share,
            IMessagingService messagingService) : base(navigation, share, dataStore, analytics, messagingService, browser, preferences)
        {
            OpenArticleCommand = new AsyncCommand<Article>(OpenArticle);
            OpenAuthorCommand = new AsyncCommand<Article>(OpenAuthor);
            SearchCategoryCommand = new Command<Article>(ExecuteCategorySearch);

            this.browser = browser;
            this.preferences = preferences;
            messagingService.Subscribe<Article>(this, "BOOKMARKED", BookMarkChanged);

        }



        private void BookMarkChanged(Article article)
        {
            // does our article collection have this article id
            var existingArcticle = Articles.FirstOrDefault(a => a.Id == article.Id);
            if (existingArcticle != null)
                existingArcticle.IsSaved = article.IsSaved;
        }

        public ICommand LoadArticlesCommand { get; set; }
        public ICommand OpenArticleCommand { get; set; }
        public ICommand OpenAuthorCommand { get; set; }
        public ICommand SearchCategoryCommand { get; set; }

        public ObservableRangeCollection<Article> Articles {
            get => articles;
            set => SetProperty(ref articles, value);
        }
        public ListState CurrentState
        {
            get => currentState;
            set => SetProperty(ref currentState, value);
        }

        private async Task OpenArticle(Article article)
        {
            if (preferences.Get(Constants.Preferences.OpenLinksInBrowser, true))
            {
                await browser.OpenAsync(article.Url, new BrowserLaunchOptions
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Show
                });
            }
            else
            {
                await navigation.GoToAsync(Constants.Navigation.Paths.ArticleView,
                                           Constants.Navigation.ParameterNames.ArticleId,
                                           article.Id);
            }
        }

        private async Task OpenAuthor(Article article)
        {
            // get author for the article
            Author author = await dataStore.SearchAuthorsAsync(article.Author);
            await navigation.GoToAsync(Constants.Navigation.Paths.Author, Constants.Navigation.ParameterNames.AuthorId, author.Id);
        }

        private void ExecuteCategorySearch(Article article)
        {
            navigation.GoToAsync(Constants.Navigation.Paths.Search, Constants.Navigation.ParameterNames.Category, WebUtility.UrlEncode(article.Category));
        }
    }
}
