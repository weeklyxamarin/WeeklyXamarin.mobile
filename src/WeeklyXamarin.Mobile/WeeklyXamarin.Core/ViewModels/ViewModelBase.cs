using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using WeeklyXamarin.Core.Services;

namespace WeeklyXamarin.Core.ViewModels
{
    public class ViewModelBase : MvvmHelpers.BaseViewModel
    {
        public ICommand CloseViewCommand { get; set; }

        protected INavigationService navigation;
        protected IAnalytics analytics;
        protected IMessagingService messagingService;

        public ViewModelBase(INavigationService navigation, IAnalytics analytics, IMessagingService messagingService)
        {
            CloseViewCommand = new AsyncCommand(ExecuteCloseViewCommand);
            this.navigation = navigation;
            this.analytics = analytics;
            this.messagingService = messagingService;
        }

        public virtual Task InitializeAsync(object parameter)
        {
            return Task.FromResult(0);
        }

        private async Task ExecuteCloseViewCommand()
        {
            await navigation.GoBackAsync();
        }
    }
}
