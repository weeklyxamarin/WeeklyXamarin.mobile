using System;
using System.Collections.Generic;
using System.Text;

namespace WeeklyXamarin.Core.Models
{
    public class Article
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string EditionId { get; set; }
        public string Author { get; set; }
        public string Id { get; internal set; }
        public string Category { get; set; }
    }
}
