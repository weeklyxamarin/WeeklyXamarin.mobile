using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Services;

namespace WeeklyXamarin.Blazor.Client.Pages.Admin;

public partial class AdminArticlesListPage
{
    [Inject] INavigationService NavigationService { get; set; }
    [Inject] IArticleRestService ArticleRestService { get; set; } = default!;
    List<Article>? Articles { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await  base.OnAfterRenderAsync(firstRender);
        if(firstRender)
        {
            Articles = await ArticleRestService.GetDraftArticles();
            StateHasChanged();
        }
    }

    public async Task Edit(Article article)
    {
        await NavigationService.GoToAsync($"/Admin/Articles/{article.Id}");
    }

    


}
