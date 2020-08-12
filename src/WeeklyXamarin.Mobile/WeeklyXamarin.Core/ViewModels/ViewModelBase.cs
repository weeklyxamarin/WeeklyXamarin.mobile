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

        public ViewModelBase(INavigationService navigation)
        {
            CloseViewCommand = new AsyncCommand(ExecuteCloseViewCommand);
            this.navigation = navigation;
        }

        private async Task ExecuteCloseViewCommand()
        {
            await navigation.GoBackAsync();
        }
    }
}
