using Shiny.Jobs;
using Shiny.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Services;

namespace WeeklyXamarin.Mobile.Services
{
    public class FetchEditionJob : Shiny.Jobs.IJob
    {
        private IDataStore _dataStore;
        private INotificationManager _notificationManager;

        public FetchEditionJob(IDataStore dataStore, INotificationManager notificationManager)
        {
            _dataStore = dataStore;
            _notificationManager = notificationManager;
        }

        public async Task<bool> Run(JobInfo jobInfo, CancellationToken cancelToken)
        {
            // always check the server

            bool hasUpdate =  await _dataStore.PreloadNextEdition();

            if (hasUpdate)
            {
                var editions = await _dataStore.GetEditionsAsync();
                var recentEdition = await _dataStore.GetEditionAsync(editions.FirstOrDefault().Id);

                var notification = new Notification
                {
                    Message = $"Editions {recentEdition?.Id} of Weekly Xamarin is available",
                    Title = "New Weekly Xamarin!"
                };

                await _notificationManager.Send(notification);
            }

            return hasUpdate;



        }
    }
}
