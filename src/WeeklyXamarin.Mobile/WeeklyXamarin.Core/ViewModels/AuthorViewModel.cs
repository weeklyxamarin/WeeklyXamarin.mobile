using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Helpers;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Services;
using Xamarin.Essentials.Interfaces;

namespace WeeklyXamarin.Core.ViewModels
{
    public class AuthorViewModel : ArticleListViewModelBase
    {
        private Author author;

        public AuthorViewModel(
            INavigationService navigation,
            IAnalytics analytics,
            IShare shareService,
            IMessagingService messagingService,
            IDataStore dataStore, IBrowser browser, IPreferences preferences) : base(navigation, analytics, dataStore, browser, preferences, shareService, messagingService)
        {        }

        public Author Author { get => author; private set => SetProperty(ref author, value); }

        public override async Task InitializeAsync(object parameter)
        {
            await base.InitializeAsync(parameter);
            var authorId = parameter as string;
            Author = await dataStore.GetAuthorAsync(authorId);
        }

        private CancellationTokenSource source = new CancellationTokenSource ();
        private ListState currentState;

        public async Task LoadArticles()
        {
            CurrentState = ListState.Loading;
            await Task.Delay(10);

            source.Cancel();
            source = new CancellationTokenSource();
            Articles.Clear();
            IAsyncEnumerable<Article> articlesAsync;

            articlesAsync = dataStore.GetArticleForAuthorAsync(Author.Name, source.Token);

            await foreach (Article article in articlesAsync)
            {
                Articles.Add(article);
                CurrentState = ListState.None;
                await Task.Delay(10);
            }
            CurrentState = Articles.Any() ? ListState.None : ListState.Empty;
        }

    }
}
