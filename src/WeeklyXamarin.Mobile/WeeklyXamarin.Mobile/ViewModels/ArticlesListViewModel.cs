using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using WeeklyXamarin.Mobile.Models;

namespace WeeklyXamarin.Mobile.ViewModels
{
    public class ArticlesListViewModel : BaseViewModel
    {
        public ObservableCollection<Article> Articles{ get; set; }
        public Command LoadArticlesCommand { get; set; }
        public string EditionId { get; }

        public ArticlesListViewModel(string editionId)
        {
            EditionId = editionId;
            Title = "Articles";
            Articles = new ObservableCollection<Article>();
            LoadArticlesCommand = new Command(async () => await ExecuteLoadArticlesCommand());
        }
        public ArticlesListViewModel()
        {

        }

        async Task ExecuteLoadArticlesCommand()
        {
            IsBusy = true;

            try
            {
                Articles.Clear();
                var articles = await DataStore.GetArticlesForEditionAsync(EditionId, true);
                foreach (var item in articles)
                {
                    Articles.Add(item);
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