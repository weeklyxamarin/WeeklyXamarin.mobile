using MatBlazor;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Services;

namespace WeeklyXamarin.Blazor.Client.Pages
{
    public partial class LookupUrl : ComponentBase
    {
        [Inject] IArticleRestService ArticleRestService { get; set; } = default!;
        [Inject] ICuratedRestService CuratedRestService { get; set; } = default!;
        [Inject] IDataStore DataStore { get; set; } = default!;
        [Inject] IMatDialogService MatDialogService { get; set; } = default!;
        [Inject] INavigationService NavigationService { get; set; } = default!;
        string? Url { get; set; }
        Article? Article { get; set; }
        List<string>? Categories { get; set; }
        List<string>? AuthorNames { get; set; }
        string CuratedStatusMessage { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                Categories = (await DataStore.GetCategories()).Select(x => x.Name).ToList();
                AuthorNames = (await DataStore.GetAuthorsAsync()).Select(x => x.Name).ToList();
            }
        }

        public async Task LoadArticle()
        {
            var article = await ArticleRestService.GetArticleDetailsFromUrl(Url);
            if (article.IsPublished)
            {
                if (await MatDialogService.ConfirmAsync($"This article is already published in {article.EditionId}, would you like to see it now?"))
                {
                    await NavigationService.GoToAsync($"/Edition/{article.EditionId}");
                }
            }
            else if (article.IsProcessed)
            {
                if (await MatDialogService.ConfirmAsync($"This article is already processed. Do you want to see it now?"))
                {
                    await NavigationService.GoToAsync($"/Admin/Articles/{article.Id}");
                }
            }
            else
                Article = article;

            CuratedStatusMessage = "";

        }

        public void NewArticle()
        {
            Article = new Article();
            CuratedStatusMessage = "";
        }

        public async Task AuthorSelected(string s)
        {
            if (Article != null)
                Article.Author = s;
        }

        public async Task PostToCurated()
        {
            CuratedStatusMessage = "Select an Article First";
            if (Article != null)
            {
                CuratedStatusMessage = "Posting to Curated";
                var result = await CuratedRestService.PostArticleToCurated(Article);
                CuratedStatusMessage = result;

            }

        }

        public async Task PostToStorage()
        {
            CuratedStatusMessage = "Select an Article First";
            if (Article != null)
            {
                CuratedStatusMessage = "Posting to Storage";
                var result = await ArticleRestService.PostArticle(Article);
                CuratedStatusMessage = "";

            }

        }


    }
}
