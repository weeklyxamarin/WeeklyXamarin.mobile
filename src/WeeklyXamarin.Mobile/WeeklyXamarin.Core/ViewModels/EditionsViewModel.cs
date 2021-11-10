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
using System.Collections.Generic;
using Xamarin.Essentials.Interfaces;

namespace WeeklyXamarin.Core.ViewModels
{
    public class EditionsViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Edition> Editions { get; set; }
        public ICommand LoadEditionsCommand { get; set; }
        public ICommand OpenEditionCommand { get;  set; }
        IDataStore dataStore;

        public EditionsViewModel(INavigationService navigation, IAnalytics analytics, IDataStore dataStore,
            IMessagingService messagingService, IBrowser browser, IPreferences preferences) : base(navigation, analytics, messagingService, browser, preferences)
        {
            Title = "Editions";
            Editions = new ObservableRangeCollection<Edition>();
            LoadEditionsCommand = new AsyncCommand(ExecuteLoadEditionsCommand);
            OpenEditionCommand = new AsyncCommand<Edition>(OpenEdition);
            this.dataStore = dataStore;
        }

        private async Task OpenEdition(Edition edition)
        {
            await navigation.GoToAsync(Constants.Navigation.Paths.Articles, Constants.Navigation.ParameterNames.EditionId, edition.Id);
        }

        public async Task ExecuteLoadEditionsCommand()
        {
            try
            {
                Editions.Clear();
                var editions = await dataStore.GetEditionsAsync(true);
                Editions.AddRange(editions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                analytics.TrackError(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}