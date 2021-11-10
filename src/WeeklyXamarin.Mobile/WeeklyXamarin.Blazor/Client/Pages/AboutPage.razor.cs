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
        [Inject] public AboutViewModel? ViewModel { get; set; }
        [Inject] public AcknowledgementsViewModel? AcknowledgementsViewModel { get; set; }
    }
}
