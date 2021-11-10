using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeeklyXamarin.Core.ViewModels;

namespace WeeklyXamarin.Blazor.Client.Pages
{
    public partial class ArticlesListPage
    {
        [Parameter]
        public string? EditionId { get; set; }
        [Inject]
        public ArticlesListViewModel? ViewModel { get; set; }

        protected override void OnParametersSet()
        {
            ViewModel!.EditionId = EditionId;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if(firstRender)
            {
                await ViewModel!.ExecuteLoadArticlesCommand();
                StateHasChanged();
            }
        }
    }
}
