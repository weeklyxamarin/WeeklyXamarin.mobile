using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace WeeklyXamarin.Mobile.Effects
{
    public class ScrollNotifierEffect : RoutingEffect
    {
        public event Action<Object, ScrolledEventArgs> ScrollChanged;

        public ScrollNotifierEffect() : base($"WeeklyXamarin.{nameof(ScrollNotifierEffect)}")
        {

        }

        public void OnScrollEffect(object sender, ScrolledEventArgs e)
        {
            ScrollChanged?.Invoke(sender, e);
        }
    }
}
