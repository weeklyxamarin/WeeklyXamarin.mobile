using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Helpers;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Services;
using WeeklyXamarin.Core.ViewModels;

namespace WeeklyXamarin.Blazor.Client.Pages
{
    public partial class SearchPage : ComponentBase
    {

        [Inject]
        public IDataStore dataStore { get; set; }

        public ListState CurrentState { get; set; }
        public List<Article> Articles { get; set; } = new List<Article>();

        public string SearchText { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
        private CancellationTokenSource source = new CancellationTokenSource();
        public async Task SearchArticles()
        {
            CurrentState = ListState.Loading;
            source.Cancel();
            source = new CancellationTokenSource();
            Articles.Clear();
            IAsyncEnumerable<Article> articlesAsync;

            articlesAsync = dataStore.GetArticleFromSearchAsync(SearchText, null, source.Token);

            await foreach (Article article in articlesAsync)
            {
                Articles.Add(article);
                await Task.Delay(10);
                StateHasChanged();
            }
            CurrentState = ListState.None;

        }
    }
}
