using System;
using System.Collections.Generic;
using System.Text;

namespace WeeklyXamarin.Core.Services
{
    public interface IMessagingService
    {
        void Send<TSender>(TSender sender, string message) where TSender : class;
        void Subscribe<TSender>(object subscriber, string message, Action<TSender> callback, TSender source = null) where TSender : class;
        void Unsubscribe<TSender>(object subscriber, string message) where TSender : class;
    }
}
