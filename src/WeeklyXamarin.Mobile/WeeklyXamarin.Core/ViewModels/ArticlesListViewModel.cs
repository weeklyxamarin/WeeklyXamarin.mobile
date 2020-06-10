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
    public class ArticlesListViewModel : ViewModelBase
    {
        IDataStore DataStore { get; set; } = new MockDataStore();

        public ObservableCollection<Article> Articles { get; set; }
        public ICommand LoadArticlesCommand { get; set; }
        public string EditionId { get; }

        public ArticlesListViewModel(string editionId)
        {
            EditionId = editionId;
            Title = "Articles";
            Articles = new ObservableCollection<Article>();
            LoadArticlesCommand = new AsyncCommand(ExecuteLoadArticlesCommand);
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