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
        string searchText;
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
                {
                    _ = ExecuteSearchArticlesCommand();
                }
            }
        }

        string lastSearchTerm;
        private string textAtTimeOfSearching;

        public string TextAtTimeOfSearching { 
            get => textAtTimeOfSearching; 
            set => SetProperty(ref textAtTimeOfSearching, value); 
        }

        private async Task ExecuteSearchArticlesCommand()
        {
            try
            {

                if (string.Equals(lastSearchTerm, SearchText, StringComparison.InvariantCultureIgnoreCase))
                    return;
                lastSearchTerm = SearchText;
                if (IsBusy) return; //don't run a search if one is already in progress
                IsBusy = true;
                Articles.Clear();

                if (SearchText?.Length > 1)
                {
                    CurrentState = ListState.Loading;
                    textAtTimeOfSearching = SearchText;
                    var articlesAsync = dataStore.GetArticleFromSearchAsync(SearchText);
                    await foreach (Article article in articlesAsync)
                    {
                        if (string.Equals(lastSearchTerm, SearchText, StringComparison.InvariantCultureIgnoreCase))
                        {
                            Articles.Add(article);
                        }
                        else
                        {
                            Articles.Clear();
                            return;
                        }
                        CurrentState = ListState.None; // show the results
                    }
                    //lastSearchTerm =  SearchText;
                    if (Articles.Count == 0)
                        CurrentState = ListState.Empty; // you found nada
                }
                else
                {
                    Articles.Clear();
                    CurrentState = ListState.None; // go to collectionview empty state
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
