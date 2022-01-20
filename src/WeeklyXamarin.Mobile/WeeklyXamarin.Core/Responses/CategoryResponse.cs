using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WeeklyXamarin.Core.Models;

namespace WeeklyXamarin.Core.Responses
{
    public class CategoryResponse
    {
        public List<Category> Categories { get; set; }

        [JsonIgnore]
        public DateTime FetchedDate { get; set; }

        [JsonIgnore]
        public bool IsStale => FetchedDate > DateTime.UtcNow.AddMinutes(-5);
    }
}
//nice