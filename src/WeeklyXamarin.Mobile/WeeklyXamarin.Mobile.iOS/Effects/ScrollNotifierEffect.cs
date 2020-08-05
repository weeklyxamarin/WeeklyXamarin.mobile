using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using WeeklyXamarin.Mobile.iOS.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("WeeklyXamarin")]
[assembly: ExportEffect(typeof(ScrollNotifierEffect), nameof(ScrollNotifierEffect))]
namespace WeeklyXamarin.Mobile.iOS.Effects
{
    public class ScrollNotifierEffect : PlatformEffect
    {
        WeeklyXamarin.Mobile.Effects.ScrollNotifierEffect effect;

        protected override void OnAttached()
        {
            // get the control
            var control = Control as WebKit.WKWebView;

            if (control != null)
            {
                System.Diagnostics.Debug.WriteLine("Connected Effect");
                Effect thisEffect = Element.Effects.FirstOrDefault(e => e is Mobile.Effects.ScrollNotifierEffect);
                effect = (WeeklyXamarin.Mobile.Effects.ScrollNotifierEffect)thisEffect;
                control.ScrollView.Scrolled += ScrollView_Scrolled;
            }
        }

        private void ScrollView_Scrolled(object sender, EventArgs e)
        {
            double yPos = ((UIScrollView)sender).ContentOffset.Y;
            yPos /= Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density;
            var args = new ScrolledEventArgs(0, yPos);

            // call this into shared effect
            effect.OnScrollEffect(sender, args);
        }

        protected override void OnDetached()
        {
        }
    }
}