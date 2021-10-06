using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeeklyXamarin.Core.ViewModels;

namespace WeeklyXamarin.Blazor.Client.Pages
{
    public partial class EditionsPage
    {
        [Inject] public EditionsViewModel? ViewModel {get;set;}

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
				if (ViewModel is not null)
				{
					await ViewModel.ExecuteLoadEditionsCommand();
				}
                StateHasChanged();
            }
        }

    }
}
