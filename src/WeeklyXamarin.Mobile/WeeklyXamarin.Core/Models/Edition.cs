using System;
using System.Collections.Generic;

namespace WeeklyXamarin.Core.Models
{
    public class Edition
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime UpdatedTimeStamp { get; set; }
        public string Summary { get; set; }
        public DateTime PublishDate { get; set; }
        public string Curators { get; set; }
        public List<Article> Articles { get; set; }
    }
}
