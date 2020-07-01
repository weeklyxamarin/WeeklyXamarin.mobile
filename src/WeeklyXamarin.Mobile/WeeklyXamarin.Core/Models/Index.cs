using System;
using System.Collections.Generic;
using WeeklyXamarin.Core.Models;

namespace WeeklyXamarin.Core.Models
{
    public class Index
    {
        public DateTime UpdatedTimeStamp { get; set; }
        public List<Edition> Editions { get; set; }
        public DateTime FetchedDate { get; set; }
    }
}
