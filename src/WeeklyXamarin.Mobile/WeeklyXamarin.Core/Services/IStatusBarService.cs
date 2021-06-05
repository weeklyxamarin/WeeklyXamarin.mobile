using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace WeeklyXamarin.Core.Services
{
    public interface IStatusBarService
    {
        void SetStatusBarColor(Color color, bool darkStatusBarTint);
    }
}
