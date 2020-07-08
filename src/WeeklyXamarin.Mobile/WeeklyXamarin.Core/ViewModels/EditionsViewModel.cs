using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Services;
using WeeklyXamarin.Core.Helpers;

namespace WeeklyXamarin.Core.ViewModels
{
    public class EditionsViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Edition> Editions { get; set; }
        public ICommand LoadEditionsCommand { get; set; }
        public ICommand OpenEditionCommand { get;  set; }
        IDataStore dataStore;
        INavigationService navigationService;

        public EditionsViewModel(IDataStore dataStore, INavigationService navigationService)
        {
            Title = "Editions";
            Editions = new ObservableRangeCollection<Edition>();
            LoadEditionsCommand = new AsyncCommand(ExecuteLoadEditionsCommand);
                OpenEditionCommand = new AsyncCommand<Edition>(OpenEdition);
            this.dataStore = dataStore;
            this.navigationService = navigationService;
        }

        private async Task OpenEdition(Edition edition)
        {
            await navigationService.GoToAsync(Constants.Navigation.Paths.Articles, Constants.Navigation.ParameterNames.EditionId, edition.Id);
        }

        async Task ExecuteLoadEditionsCommand()
        {
            IsBusy = true;

            try
            {
                Editions.Clear();
                var editions = await dataStore.GetEditionsAsync(true);
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