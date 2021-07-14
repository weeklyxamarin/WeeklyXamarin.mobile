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
    }
}
