using System;
using System.Collections.Generic;
using System.Text;

namespace WeeklyXamarin.Core.Responses
{
    public class Response<T>
    {
        public bool IsStale { get; set; }
        public DateTime FetchedDate { get; set; }
        public T Data { get; set; }
    }
}
