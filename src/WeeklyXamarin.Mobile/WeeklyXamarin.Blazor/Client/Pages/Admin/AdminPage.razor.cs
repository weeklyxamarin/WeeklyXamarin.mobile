using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Services;

namespace WeeklyXamarin.Blazor.Client.Pages.Admin
{
    public partial class AdminPage
    {
        public List<Article>? Articles { get; private set; }
        [Inject] IEditionRestService EditionRestService { get; set; } = default!;
        [Inject] IArticleRestService ArticleRestService { get; set; } = default!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                Articles = await ArticleRestService.GetDraftArticles();
                StateHasChanged();
            }
        }
    }
}
