using System;
using System.Collections.Generic;
using System.Text;
using WeeklyXamarin.Core.Services;
using Xamarin.Forms;

namespace WeeklyXamarin.Mobile.Services
{
    public class MessagingService : IMessagingService
    {
        public void Send<TSender>(TSender sender, string message) where TSender : class
        {
            MessagingCenter.Send(sender, message);
        }

        public void Subscribe<TSender>(object subscriber, string message, Action<TSender> callback, TSender source = null) where TSender : class
        {
            MessagingCenter.Subscribe(subscriber, message, callback, source);
        }

        public void Unsubscribe<TSender>(object subscriber, string message) where TSender : class
        {
            MessagingCenter.Unsubscribe<TSender>(subscriber, message);
        }
    }
}
