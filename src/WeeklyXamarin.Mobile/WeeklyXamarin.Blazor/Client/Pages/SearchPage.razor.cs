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
        public IDataStore DataStore { get; set; } = default!;

        [Inject]
        public INavigationService Navigation { get; set; } = default!;

        public ListState CurrentState { get; set; }
        public List<Article> Articles { get; set; } = new List<Article>();
        
        public string? SearchText { get; set; }
        public List<Category> Categories { get; private set; } = new List<Category>();
        public Category? SearchCategory { get; set; }
        public string SearchResultText
        {
            get
            {
                return SearchCategory == null ? $"No results for '{SearchText}'." : $"No results for '{SearchText}' in '{SearchCategory.Name}'.";
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            
            Categories = (await DataStore.GetCategories()).ToList();
            Categories.Insert(0, new Category());
        }
        private CancellationTokenSource source = new();
        public async Task SearchArticles()
        {
            CurrentState = ListState.Loading;
            StateHasChanged();
            await Task.Delay(10);

            source.Cancel();
            source = new CancellationTokenSource();
            Articles.Clear();
            IAsyncEnumerable<Article> articlesAsync;

            articlesAsync = DataStore.GetArticleFromSearchAsync(SearchText, SearchCategory?.Name, source.Token);

            await foreach (Article article in articlesAsync)
            {
                Articles.Add(article);
                CurrentState = ListState.None;
                StateHasChanged();
                await Task.Delay(10);
            }
            CurrentState = Articles.Any() ? ListState.None : ListState.Empty;

        }

        public async Task OpenAuthor(string authorName)
        {
            Author author = await DataStore.SearchAuthorsAsync(authorName);
            await Navigation.GoToAsync($"{Constants.Navigation.Paths.Author}/{author.Id}");
        }
    }
}
