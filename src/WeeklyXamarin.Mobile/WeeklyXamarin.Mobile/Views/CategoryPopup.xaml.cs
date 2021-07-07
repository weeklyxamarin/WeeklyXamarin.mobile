using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Models;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeeklyXamarin.Mobile.Views
{
    public partial class CategoryPopup : Popup
    {
        private IEnumerable<Category> _categories;

        public CategoryPopup(IEnumerable<Category> categories )
        {
            InitializeComponent();
            _categories = categories;
        }



        public void DismissClicked(object sender, EventArgs e)
        {
            Dismiss("This is my return values");
        }
    }
}