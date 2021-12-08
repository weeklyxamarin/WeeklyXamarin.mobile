using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyXamarin.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeeklyXamarin.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AcknowledgementsPage : PageBase<AcknowledgementsViewModel>
    {
        public AcknowledgementsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ViewModel.InitializeAsync(null);
        }
    }
}