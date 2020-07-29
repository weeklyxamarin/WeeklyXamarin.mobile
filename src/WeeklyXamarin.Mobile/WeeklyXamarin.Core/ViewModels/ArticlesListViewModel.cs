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

namespace WeeklyXamarin.Core.ViewModels
{
    public class ArticlesListViewModel : ViewModelBase
    {
        readonly IDataStore dataStore;

        public ObservableRangeCollection<Article> Articles { get; set; } = new ObservableRangeCollection<Article>();
        public AsyncCommand<bool> LoadArticlesCommand { get; set; }
        public ICommand OpenArticleCommand { get; set; }
        public string EditionId { get; set;  }

        public ArticlesListViewModel(INavigationService navigation, IDataStore dataStore) : base(navigation)
        {
            Title = "Articles";
            LoadArticlesCommand = new AsyncCommand<bool>(ExecuteLoadArticlesCommand, CanRefresh);
            OpenArticleCommand = new AsyncCommand<Article>(OpenArticle);
            this.dataStore = dataStore;
        }

        private bool CanRefresh(object arg)
        {
            return !IsBusy;
        }

        private async Task OpenArticle(Article article)
        {
            var navigationParameters = new Dictionary<string,  string>
            {
                {Constants.Navigation.ParameterNames.ArticleId, article.Id },
                {Constants.Navigation.ParameterNames.EditionId, EditionId },
            };

            await navigation.GoToAsync(Constants.Navigation.Paths.ArticleDetail, navigationParameters);
        }

        async Task ExecuteLoadArticlesCommand(bool forceRefresh = false)
        {
            IsBusy = true;

            try
            {
                Articles.Clear();
                var edition = await dataStore.GetEditionAsync(EditionId,forceRefresh);
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