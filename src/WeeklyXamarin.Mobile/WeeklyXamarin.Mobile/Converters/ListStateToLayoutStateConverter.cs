using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using WeeklyXamarin.Core.Helpers;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace WeeklyXamarin.Mobile.Converters
{
    public class ListStateToLayoutStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (LayoutState)value;
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (ListState)value;
        }
    }
}
