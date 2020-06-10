using System;
using System.Collections.Generic;
using System.Text;

namespace WeeklyXamarin.Core.Services
{
    public class Container
    {
        public static Container Instance { get; } = new Container();
        public IServiceProvider ServiceProvider { get; set;  }
    }
}
