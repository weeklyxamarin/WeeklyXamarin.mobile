using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WeeklyXamarin.Core.Models
{
    public class Edition
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime UpdatedTimeStamp { get; set; }
        public string Summary { get; set; }
        public string Introduction { get; set; }
        public DateTime PublishDate { get; set; }
        public string PublishUrl { get; set; }
        public string Curators { get; set; }
        public bool IsPublished { get; set; }

        [IgnoreDataMember]
        public List<Article> Articles { get; set; }
    }

}
