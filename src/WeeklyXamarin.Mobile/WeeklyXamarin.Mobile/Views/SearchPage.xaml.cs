using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Helpers;
using WeeklyXamarin.Core.ViewModels;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeeklyXamarin.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : PageBase<SearchViewModel>
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        private void Picker_Unfocused(object sender, FocusEventArgs e)
        {
            ViewModel.SearchArticlesCommand.Execute(null);
        }
    }
}