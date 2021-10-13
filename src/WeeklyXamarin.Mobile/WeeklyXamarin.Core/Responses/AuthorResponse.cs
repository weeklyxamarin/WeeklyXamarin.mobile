using System;
using System.Collections.Generic;
using System.Text;
using WeeklyXamarin.Core.Models;

namespace WeeklyXamarin.Core.Responses
{
    public class AuthorResponse
    {
        public List<Author> Authors { get; set; }
        public bool IsStale { get; set; } = false;
        public DateTime FetchedDate { get; set; }
    }
}
