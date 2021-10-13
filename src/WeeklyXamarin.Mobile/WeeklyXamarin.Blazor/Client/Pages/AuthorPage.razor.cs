using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Helpers;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Services;
using WeeklyXamarin.Core.ViewModels;

namespace WeeklyXamarin.Blazor.Client.Pages
{
    public partial class AuthorPage
    {
        [Inject]
        public AuthorViewModel? ViewModel { get; set; }
        [Inject]
        public IDataStore? dataStore { get; set; }
        [Inject]
        public INavigationService? navigation { get; set; }
        public List<Article> Articles { get; set; } = new List<Article>();

        [Parameter]
        public string Id { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await ViewModel!.InitializeAsync(Id);
                await LoadArticles();
                StateHasChanged();
            }
        }

        public async Task OpenAuthor(string authorName)
        {
            Author author = await dataStore.SearchAuthorsAsync(authorName);
            await navigation.GoToAsync($"{Constants.Navigation.Paths.Author}/{author.Id}");
        }

        private CancellationTokenSource source = new CancellationTokenSource();
        private async Task LoadArticles()
        {
            ViewModel.CurrentState = ListState.Loading;
            StateHasChanged();
            await Task.Delay(10);

            source.Cancel();
            source = new CancellationTokenSource();
            Articles.Clear();
            IAsyncEnumerable<Article> articlesAsync;

            articlesAsync = dataStore!.GetArticleFromSearchAsync(ViewModel.Author.Name, null, source.Token);

            await foreach (Article article in articlesAsync)
            {
                Articles.Add(article);
                ViewModel.CurrentState = ListState.None;
                StateHasChanged();
                await Task.Delay(10);
            }
            ViewModel.CurrentState = Articles.Any() ? ListState.None : ListState.Empty;

        }
    }
}
