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
            Article = await ArticleRestService.GetArticleDetailsFromUrl(Url);
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

    }
}
