﻿using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Microsoft.Extensions.DependencyInjection;
using Sharpnado.MaterialFrame.iOS;
using Shiny;
using UIKit;
using WeeklyXamarin.Core.Services;
using WeeklyXamarin.Mobile.iOS.Services;

namespace WeeklyXamarin.Mobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            // in your FinishedLaunching method
            iOSShinyHost.Init(new WeeklyXamarin.Mobile.Services.WeeklyXamarinStartup());

            global::Xamarin.Forms.Forms.Init();
            iOSMaterialFrameRenderer.Init();
            LoadApplication(new App(ConfigureServices));

            return base.FinishedLaunching(app, options);
        }


        public override void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
        {
            Shiny.Jobs.JobManager.OnBackgroundFetch(completionHandler);
        }

        private void ConfigureServices(ServiceCollection container)
        {
            container.AddSingleton<IStatusBarService, StatusBarService>(_ => new StatusBarService());
        }
    }
}
