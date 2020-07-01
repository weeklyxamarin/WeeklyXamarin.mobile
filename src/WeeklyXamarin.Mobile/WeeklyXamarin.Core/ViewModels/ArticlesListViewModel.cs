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
    public class ArticlesListViewModel : ViewModelBase
    {
        readonly IDataStore dataStore;
        readonly INavigationService navigationService;

        public ObservableRangeCollection<Article> Articles { get; set; } = new ObservableRangeCollection<Article>();
        public ICommand LoadArticlesCommand { get; set; }
        public ICommand OpenArticleCommand { get; set; }
        public string EditionId { get; set;  }

        public ArticlesListViewModel(IDataStore dataStore, INavigationService navigationService)
        {
            Title = "Articles";
            LoadArticlesCommand = new AsyncCommand(ExecuteLoadArticlesCommand);
            OpenArticleCommand = new AsyncCommand<Article>(OpenArticle);
            this.dataStore = dataStore;
            this.navigationService = navigationService;
        }

        private async Task OpenArticle(Article article)
        {
            await navigationService.GoToAsync("articledetail", "article", article.Id);
        }

        async Task ExecuteLoadArticlesCommand()
        {
            IsBusy = true;

            try
            {
                Articles.Clear();
                //var articles = await dataStore.GetArticlesForEditionAsync(EditionId, true);
                var edition = await dataStore.GetEditionAsync(EditionId);
                Articles.AddRange(edition.Articles);
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