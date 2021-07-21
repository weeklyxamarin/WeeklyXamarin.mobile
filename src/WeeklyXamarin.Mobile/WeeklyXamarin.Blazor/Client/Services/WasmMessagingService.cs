using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Services;

namespace WeeklyXamarin.Blazor.Client.Services
{
    public class WasmMessagingService : IMessagingService
    {
        public void Send<TSender>(TSender sender, string message) where TSender : class
        {
            return;
        }

        public void Subscribe<TSender>(object subscriber, string message, Action<TSender> callback, TSender source = null) where TSender : class
        {
            return;
        }

        public void Unsubscribe<TSender>(object subscriber, string message) where TSender : class
        {
            return;
        }
    }
}
