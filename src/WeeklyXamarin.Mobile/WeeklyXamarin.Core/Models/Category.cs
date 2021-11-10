using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WeeklyXamarin.Core.Models
{
    public class Category : ObservableObject
    {
        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Color => CategoryToColor(Name);
        public static string CategoryToColor(string input)
        {
            return input switch
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
        }
    }
}
