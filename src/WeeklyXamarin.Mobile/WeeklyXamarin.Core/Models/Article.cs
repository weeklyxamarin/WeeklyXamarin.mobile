using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeeklyXamarin.Core.Models
{
    public class Article : ObservableObject
    {
        private bool isSaved;
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string EditionId { get; set; }
        public string Author { get; set; }
        public string Id { get; set; }
        public string Category { get; set; }
        [JsonIgnore]
        public bool IsSaved { get => isSaved; set => SetProperty(ref isSaved, value); }

        private string SearchIndex 
        { 
            get
            {
                return $"{Title} {Description} {Author} {Category}".ToLower();
            }
        }

        internal bool Matches(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
                return true;

            var terms = searchText.Trim().ToLower().Split(' ',StringSplitOptions.RemoveEmptyEntries);
            return terms.Any(i => SearchIndex.Contains(i));
        }

        internal bool MatchesCategory(string category)
        {
            if (string.IsNullOrEmpty(category))
                return true;

            if (Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }
    }
}
