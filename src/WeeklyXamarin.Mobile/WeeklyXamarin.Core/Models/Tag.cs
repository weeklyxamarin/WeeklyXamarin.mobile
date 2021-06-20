using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WeeklyXamarin.Core.Models
{
    public class Tag
    {
        public List<Category> Categories { get; set; }

        [JsonIgnore]
        public DateTime FetchedDate { get; set; }

        [JsonIgnore]
        public bool IsStale => FetchedDate > DateTime.UtcNow.AddMinutes(-5);
    }

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
