using Microsoft.AspNetCore.Components;
using WeeklyXamarin.Core.Services;

namespace WeeklyXamarin.Blazor.Client.Pages.Admin
{
    public partial class AdminPage
    {
        [Inject] IEditionRestService EditionRestService {get;set;}
    }
}
