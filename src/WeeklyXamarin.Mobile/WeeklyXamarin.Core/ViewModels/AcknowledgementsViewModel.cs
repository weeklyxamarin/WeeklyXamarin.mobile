﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeeklyXamarin.Core.Services;
using Xamarin.Essentials.Interfaces;

namespace WeeklyXamarin.Core.ViewModels
{
    public class AcknowledgementsViewModel : ViewModelBase
    {
        public List<Acknowledgement> Acknowledgements { get; set; }

        public AcknowledgementsViewModel(INavigationService navigation, IAnalytics analytics,
            IMessagingService messagingService, IBrowser browser, IPreferences preferences) : base(navigation, analytics, messagingService, browser, preferences)
        {
            var thanks = new Acknowledgements();
            Acknowledgements = thanks.Thanks.ToList();
        }
    }
}
