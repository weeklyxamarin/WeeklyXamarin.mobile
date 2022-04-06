using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Services;

namespace WeeklyXamarin.Blazor.Client.Pages.Admin
{
    public partial class AdminAuthorListPage
    {
        [Inject] INavigationService NavigationService { get; set; }
        [Inject] IAuthorRestService AuthorRestService { get; set; } = default!;
        List<Author>? Authors { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                Authors = await AuthorRestService.GetAllAuthors();
                StateHasChanged();
            }
        }

        public async Task Edit(Author author)
        {
            await NavigationService.GoToAsync($"/Admin/Authors/{author.Id}");
            
        }


    }
}
