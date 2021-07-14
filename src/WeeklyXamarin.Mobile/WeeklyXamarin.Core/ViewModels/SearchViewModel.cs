using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WeeklyXamarin.Core.Helpers;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Services;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;

namespace WeeklyXamarin.Core.ViewModels
{
    public class SearchViewModel : ArticleListViewModelBase
    {
        string searchText;
        string _searchHeader;
        private bool _showCategory;

        public ICommand ClearCategoryCommand { get; set; } 
        public ICommand SearchArticlesCommand { get; set; }
        public SearchViewModel(INavigationService navigation, IAnalytics analytics, IDataStore dataStore, IBrowser browser, IPreferences preferences, IShare share,
            IMessagingService messagingService) : base(navigation, analytics, dataStore, browser, preferences, share, messagingService)
        {
            SearchArticlesCommand = new AsyncCommand(ExecuteSearchArticlesCommand);
            ClearCategoryCommand = new AsyncCommand(ExecuteClearCategoryCommand);
            _ = InitializeAsync();
        }

        public ObservableRangeCollection<Category> Categories { get; set; } = new ObservableRangeCollection<Category>();

        public Category SearchCategory
        {
            get => searchCategory;
            set 
            {
                SetProperty(ref searchCategory, value);
            }
        }

        public string SearchText
        {
            get => searchText;
            set
            {
                SetProperty(ref searchText, value);
                // Manual edit, search by keyword
                if (value is { Length: 0 })
                {
                    _ = ExecuteSearchArticlesCommand();
                }
            }
        }

        public string SearchResultText
        {
            get
            {
                return searchCategory == null ? $"No results for '{SearchText}'." : $"No results for '{SearchText}' in '{searchCategory.Name}'.";
            }
        }

        public string LastSearchTerm
        {
            get => lastSearchTerm;
            set => SetProperty(ref lastSearchTerm, value);
        }

        public string SearchHeader
        {
            get => _searchHeader;
            set => SetProperty(ref _searchHeader, value);
        }
        public Category LastSearchCategory { get; private set; }

        public override async Task InitializeAsync(object parameter = null)
        {
            await base.InitializeAsync(parameter);
            Categories.AddRange(await dataStore.GetCategories());
        }

        CancellationTokenSource cts = new CancellationTokenSource();

        object lid = new object();
        string lastSearchTerm = "";
        private Category searchCategory;

        private async Task ExecuteClearCategoryCommand()
        {
            SearchCategory = null;
            await ExecuteSearchArticlesCommand();
        }

        private async Task ExecuteSearchArticlesCommand()
        {
            try
            {
                var trimmedSearchTerm = SearchText?.Trim();
                if (trimmedSearchTerm == LastSearchTerm && SearchCategory == LastSearchCategory)
                    return;
                LastSearchTerm = trimmedSearchTerm;
                LastSearchCategory = SearchCategory;
                cts?.Cancel();
                lock (lid)
                {
                    Articles = new ObservableRangeCollection<Article>();
                }

                if (string.IsNullOrWhiteSpace(trimmedSearchTerm) && SearchCategory == null)
                {
                    CurrentState = ListState.None;
                    return;
                }

                cts = new CancellationTokenSource();

                if (trimmedSearchTerm?.Length > 1 || SearchCategory != null)
                {
                    CurrentState = ListState.Loading;
                    Debug.WriteLine($">> Starting Search for {trimmedSearchTerm} {SearchCategory?.Name}");

                    var resultBucket = new List<Article>();

                    var timer = new System.Timers.Timer(500);

                    timer.Elapsed += delegate
                    {
                        // if the bucket has some things
                        if (resultBucket.Count > 0)
                        {
                            DumpBucket(resultBucket, cts.Token);
                        }
                    };
                    timer.Start();

                    IAsyncEnumerable<Article> articlesAsync;

                    articlesAsync = dataStore.GetArticleFromSearchAsync(trimmedSearchTerm, SearchCategory?.Name, cts.Token);

                    await foreach (Article article in articlesAsync)
                    {
                        //lock
                        lock (lid)
                        {
                            if (!cts.IsCancellationRequested)
                                resultBucket.Add(article);
                        }

                        if (resultBucket.Count >= 20)
                        {
                            DumpBucket(resultBucket, cts.Token);
                        }
                    }

                    if (resultBucket.Count > 0)
                    {
                        DumpBucket(resultBucket, cts.Token);
                    }

                    if (Articles.Count == 0)
                    {
                        CurrentState = ListState.Empty;
                        OnPropertyChanged(nameof(SearchResultText));
                    }
                    else
                        CurrentState = ListState.None;
                    timer.Stop();

                }
                else
                {
                    CurrentState = ListState.None; // go to collectionview empty state
                }
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine($"Cancelled");
            }
            catch (Exception ex)
            {
                CurrentState = ListState.Error;
            }
        }

        private void DumpBucket(List<Article> resultBucket, CancellationToken token)
        {
            lock (lid)
            {
                var newBucketWithOldBananas = resultBucket.ToList();
                resultBucket.Clear();
                if (!token.IsCancellationRequested)
                    Articles.AddRange(newBucketWithOldBananas);
            }
            CurrentState = ListState.None; // show the results
        }
    }
}
