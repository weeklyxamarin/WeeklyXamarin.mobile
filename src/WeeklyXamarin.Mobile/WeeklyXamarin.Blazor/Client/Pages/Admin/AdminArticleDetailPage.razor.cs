using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Services;
using MatBlazor;

namespace WeeklyXamarin.Blazor.Client.Pages.Admin
{
    public partial class AdminArticleDetailPage
    {
        [Inject] IArticleRestService ArticleRestService { get; set; } = default!;

        [Inject] IDataStore DataStore { get; set; } = default!;
        [Inject] INavigationService NavigationService { get; set; } = default;
        [Inject] IMatDialogService MatDialogService{get;set;}
        [Parameter] public string Id { get; set; }
        string? Url { get; set; }
        Article? Article { get; set; }
        List<string>? Categories { get; set; }
        List<string>? AuthorNames { get; set; }

        string StatusMessage { get; set; }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                Categories = (await DataStore.GetCategories()).Select(x => x.Name).ToList();
                AuthorNames = (await DataStore.GetAuthorsAsync()).Select(x => x.Name).ToList();
                await LoadArticle();
                StateHasChanged();
            }
        }

        public async Task LoadArticle()
        {
            Article = await ArticleRestService.GetArticle(Id);
        }

        public async Task AuthorSelected(string s)
        {
            if (Article != null)
                Article.Author = s;
        }

        public async Task UpdateArticle()
        {
            if (Article != null)
            {
                StatusMessage = "Updating";
                var result = await ArticleRestService.PostArticle(Article);
                StatusMessage = "Done";
            }

        }

        public async Task DeleteArticle()
        {
            StatusMessage = "Deleting";
            if(await MatDialogService.ConfirmAsync("Delete this article?"))
            {
                var result = await ArticleRestService.DeleteArticle(Id);
                await NavigationService.GoToAsync("/Admin/Articles");
            }
            StatusMessage = string.Empty;
        }

    }
}
