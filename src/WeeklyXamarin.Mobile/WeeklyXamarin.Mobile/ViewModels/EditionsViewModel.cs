using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using WeeklyXamarin.Mobile.Models;

namespace WeeklyXamarin.Mobile.ViewModels
{
    public class EditionsViewModel : BaseViewModel
    {
        public ObservableCollection<Edition> Editions { get; set; }
        public Command LoadEditionsCommand { get; set; }

        public EditionsViewModel()
        {
            Title = "Editions";
            Editions = new ObservableCollection<Edition>();
            LoadEditionsCommand = new Command(async () => await ExecuteLoadEditionsCommand());
        }

        async Task ExecuteLoadEditionsCommand()
        {
            IsBusy = true;

            try
            {
                Editions.Clear();
                var editions = await DataStore.GetEditionsAsync(true);
                foreach (var item in editions)
                {
                    Editions.Add(item);
                }
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