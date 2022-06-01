using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Services;

namespace WeeklyXamarin.Blazor.Client.Pages.Admin;

public partial class EditionDetailPage
{
    [Inject] IArticleRestService ArticleRestService { get; set; } = default!;
    [Inject] IEditionRestService EditionRestService { get; set; } = default!;
    [Parameter] public string? Id { get; set; }

    Edition Edition { get; set; }

    public List<Article> UnassignedArticles { get; private set; }

    async Task AddArticle(Article article)
    {
        article.EditionId = Edition.Id;
        UnassignedArticles.Remove(article);
        Edition.Articles.Add(article);
        await ArticleRestService.PostArticle(article);
    }

    async Task RemoveArticle(Article article)
    {
        article.EditionId = "";
        Edition.Articles.Remove(article);
        UnassignedArticles.Add(article);
        await ArticleRestService.PostArticle(article);
    }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender)
        {
            var articles = await ArticleRestService.GetDraftArticles();
            UnassignedArticles = articles.Where(x => !x.IsPublished).ToList();
            Edition = await EditionRestService.GetEdition(Id);
            Edition.Articles = articles.Where(x => x.EditionId == Edition.Id).ToList();
            StateHasChanged();
        }
    }
}
