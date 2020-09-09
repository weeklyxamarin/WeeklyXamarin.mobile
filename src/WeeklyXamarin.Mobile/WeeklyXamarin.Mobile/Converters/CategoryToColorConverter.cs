using System;
using System.Globalization;
using Xamarin.Forms;


namespace WeeklyXamarin.Mobile.Converters
{
    [ValueConversion(typeof(string), typeof(Color))]
    public class CategoryToColorConverter : IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string == false)
            {
                return default(Color);
            }

            var input = (string)value;

            var resourceName = input switch
            {
                // todo: add some colors
                "XAMARIN FORMS" => "NavyBlueDark",
                "DESIGN" => "OrangeDark",
                _ => "SkyBlueDark"
            };

            return (Color)Application.Current.Resources[resourceName];

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}