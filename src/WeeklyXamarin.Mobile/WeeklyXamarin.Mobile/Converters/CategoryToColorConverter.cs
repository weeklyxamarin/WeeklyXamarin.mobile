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
                "XAMARIN FORMS" => "BlueLight",
                "DESIGN" => "OrangeLight",
                "DEVOPS" => "LimeLight",
                "TOOLS" => "MagentaLight",
                "ANDROID" => "SkyBlueLight",
                "IOS" => "GreenLight",
                "PODCASTS & VIDEOS" => "WatermelonLight",
                "NEWS" => "PowderBlueDark",
                "TESTING" => "RedLight",
                "ANALYTICS" => "RedLight",
                "APP OF THE WEEK" => "PinkLight",
                "CODE" => "TealLight",
                "GETTING STARTED" => "YellowDark",
                _ => "SkyBlueLight"
            };

            return (Color)Application.Current.Resources[resourceName];

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}