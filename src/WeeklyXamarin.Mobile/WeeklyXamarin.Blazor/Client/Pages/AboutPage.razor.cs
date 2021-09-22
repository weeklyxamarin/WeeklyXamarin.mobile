using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MonkeyCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeeklyXamarin.Core.ViewModels;

namespace WeeklyXamarin.Blazor.Client.Pages
{
    public partial class AboutPage
    {
        [Inject] public AboutViewModel ViewModel { get; set; }
        [Inject] public AcknowledgementsViewModel AcknowledgementsViewModel { get; set; }
        [Inject] public IJSRuntime jSRuntime { get; set; }
        [Inject] IBarrel Barrel { get; set; }
        public bool IsDarkMode { get; set; } 
        public async Task ThemeSwitchChanged(ChangeEventArgs e)
        {
            Barrel.Add("is-dark-theme", e.Value, default);
            await jSRuntime.InvokeVoidAsync("switchTheme", (bool)e.Value);
            IsDarkMode = (bool)e.Value;
        }

        //protected override async Task OnInitializedAsync()
        //{
        //    await  base.OnInitializedAsync();
        //    IsDarkMode = Barrel.Get<bool>("is-dark-theme");
        //}
    }
}
