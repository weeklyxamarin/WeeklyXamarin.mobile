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
        string? CuratedByLine { get; set; }
        string? CuratedKey { get; set; }
        string? CuratedPublicationId { get; set; }


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
            if (Article.Author != null)
                await UpdateByLine(Article.Author);

        }

        private async Task UpdateByLine(string authorName)
        {
            Author author = await DataStore.SearchAuthorsAsync(authorName);
            CuratedByLine = $"[**{Article?.Title}**]({Article?.Url}) by [{author.Name}]({author.PreferredContact})";
        }

        public async Task AuthorSelected(string s)
        {
            if (Article != null)
                Article.Author = s;
            await UpdateByLine(s);
        }

        public async Task PostToCurated()
        {
            if (Article != null)
            {
                // create article suitable for curated
                Article articleToPost = new Article();
                articleToPost.Title = Article.Title;
                articleToPost.Url = Article.Url;

                var curatedDescription = new StringBuilder();
                curatedDescription.AppendLine(Article.Description);
                curatedDescription.AppendLine();
                curatedDescription.AppendLine(CuratedByLine);
                articleToPost.Description = curatedDescription.ToString();

                await CuratedRestService.PostArticleToCurated(articleToPost);
            }

        }

    }
}
