using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Services;

namespace WeeklyXamarin.Core.ViewModels
{
    public class EditionsViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Edition> Editions { get; set; }
        public ICommand LoadEditionsCommand { get; set; }
        IDataStore DataStore { get; set; } = new MockDataStore();

        public EditionsViewModel()
        {
            Title = "Editions";
            Editions = new ObservableRangeCollection<Edition>();
            LoadEditionsCommand = new AsyncCommand(ExecuteLoadEditionsCommand);
        }

        async Task ExecuteLoadEditionsCommand()
        {
            IsBusy = true;

            try
            {
                Editions.Clear();
                var editions = await DataStore.GetEditionsAsync(true);
                Editions.AddRange(editions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}