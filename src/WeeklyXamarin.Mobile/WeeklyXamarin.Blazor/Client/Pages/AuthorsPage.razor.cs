using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeeklyXamarin.Core.ViewModels;

namespace WeeklyXamarin.Blazor.Client.Pages
{
    public partial class AuthorsPage
    {
        [Inject]
        public AuthorsViewModel? ViewModel { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await ViewModel!.InitializeAsync(null);
                StateHasChanged();
            }
        }
    }
}
