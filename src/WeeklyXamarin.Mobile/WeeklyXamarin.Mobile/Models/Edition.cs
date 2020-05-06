using System;
using System.Collections.Generic;
using System.Text;

namespace WeeklyXamarin.Mobile.Models
{
    public class Edition
    {
        public string Id { get; set; }
        public string Summary { get; set; }
        public DateTime PublishDate { get; set; }
        public string Curators { get; set; }
    }
}
