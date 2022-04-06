using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Services;
using MatBlazor;
using System;

namespace WeeklyXamarin.Blazor.Client.Pages.Admin
{
    public partial class AdminAuthorDetailPage
    {
        [Inject] IAuthorRestService AuthorRestService { get; set; } = default!;

        [Inject] IDataStore DataStore { get; set; } = default!;
        [Inject] INavigationService NavigationService { get; set; } = default;
        [Inject] IMatDialogService MatDialogService { get; set; }
        [Parameter] public string Id { get; set; }
        string? Url { get; set; }
        Author? Author { get; set; }

        string StatusMessage { get; set; }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                await LoadAuthor();
                StateHasChanged();
            }
        }

        public async Task LoadAuthor()
        {
            if(string.IsNullOrWhiteSpace(Id))
            {
                Author = new Author
                {
                    Id = Guid.NewGuid().ToString()
                };
            }
            else
            {
                Author = await AuthorRestService.GetAuthor(Id);
            }
        } 

        public async Task UpdateAuthor()
        {
            if (Author != null)
            {
                StatusMessage = "Updating";
                var result = await AuthorRestService.PostAuthor(Author);
                StatusMessage = "Done";
            }

        }

        public async Task DeleteAuthor()
        {
            StatusMessage = "Deleting";
            if (await MatDialogService.ConfirmAsync("Delete this Author?"))
            {
                var result = await AuthorRestService.DeleteAuthor(Id);
                await NavigationService.GoToAsync("/Admin/Authors");
            }
            StatusMessage = string.Empty;
        }

    }
}
