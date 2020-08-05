using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using WeeklyXamarin.Mobile.Droid.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("WeeklyXamarin")]
[assembly: ExportEffect(typeof(ScrollNotifierEffect), nameof(ScrollNotifierEffect))]
namespace WeeklyXamarin.Mobile.Droid.Effects
{
    public class ScrollNotifierEffect : PlatformEffect
    {
        WeeklyXamarin.Mobile.Effects.ScrollNotifierEffect effect;
        protected override void OnAttached()
        {
            // get the control
            var control = Control as Android.Webkit.WebView;

            if (control != null)
            {
                System.Diagnostics.Debug.WriteLine("Connected Effect");
                Effect thisEffect = Element.Effects.FirstOrDefault(e => e is Mobile.Effects.ScrollNotifierEffect);
                effect = (WeeklyXamarin.Mobile.Effects.ScrollNotifierEffect)thisEffect;
                control.ScrollChange += Control_ScrollChange;
            }
        }

        private void Control_ScrollChange(object sender, Android.Views.View.ScrollChangeEventArgs e)
        {
            double yPos = e.ScrollY;
            yPos /= Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density;
            var args = new ScrolledEventArgs(0, yPos);

            // call this into shared effect
            effect.OnScrollEffect(sender, args);
        }

        protected override void OnDetached()
        {
            System.Diagnostics.Debug.WriteLine("Disconnected Effect");
            //control.ScrollChange -= Control_ScrollChange;

        }
    }
}