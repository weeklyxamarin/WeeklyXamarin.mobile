using System;
using System.Collections.Generic;
using System.Text;

namespace WeeklyXamarin.Core.Responses
{
    public class Response<T>
    {
        public bool IsStale => DateTime.UtcNow > FetchedDate.AddMinutes(5);
        public DateTime FetchedDate { get; set; }
        public T Data { get; set; }
    }
}
