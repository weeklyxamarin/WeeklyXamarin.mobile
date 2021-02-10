using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeeklyXamarin.Core.Helpers;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Services;
using Xamarin.Essentials.Interfaces;

namespace WeeklyXamarin.Core.ViewModels
{
    public class SearchViewModel : ArticleListViewModelBase
    {
        private string searchText;
        public ICommand SearchArticlesCommand { get; set; }
        public SearchViewModel(INavigationService navigation, IAnalytics analytics, IDataStore dataStore, IBrowser browser, IPreferences preferences, IShare share) : base(navigation, analytics, dataStore, browser, preferences, share)
        {
            SearchArticlesCommand = new AsyncCommand(ExecuteSearchArticlesCommand);
        }

        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                if (value is { Length: 0 })
                    _ = ExecuteSearchArticlesCommand();
            }
        }

        private async Task ExecuteSearchArticlesCommand()
        {
            try
            {
                Articles.Clear();
                if (SearchText?.Length > 1)
                {
                    CurrentState = ListState.Loading;
                    var articlesAsync = dataStore.GetArticleFromSearchAsync(SearchText);
                    await foreach (Article article in articlesAsync)
                    {
                        Articles.Add(article);
                        CurrentState = ListState.None;
                    }
                    if (Articles.Count == 0)
                        CurrentState = ListState.Empty; // you found nada
                }
                else
                {
                    CurrentState = ListState.Empty; // enter 2 or characters
                }
            }
            catch (Exception ex)
            {
                // display something here
                // log something here
                CurrentState = ListState.Error;
            }
            finally
            {
                IsBusy = false;
            }

        }

    }
}
