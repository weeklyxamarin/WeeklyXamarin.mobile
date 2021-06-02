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
        public ICommand SearchArticlesCommand { get; set; }
        public SearchViewModel(INavigationService navigation, IAnalytics analytics, IDataStore dataStore, IBrowser browser, IPreferences preferences, IShare share,
            IMessagingService messagingService) : base(navigation, analytics, dataStore, browser, preferences, share, messagingService)
        {
            SearchArticlesCommand = new AsyncCommand(ExecuteSearchArticlesCommand);
        }

        public string SearchText
        {
            get => searchText;
            set
            {
                SetProperty(ref searchText, value);
                if (value is { Length: 0 })
                {
                    _ = ExecuteSearchArticlesCommand();
                }
            }
        }

        public string LastSearchTerm 
        { 
            get => lastSearchTerm;
            set => SetProperty(ref lastSearchTerm, value); 
        }

        CancellationTokenSource cts = new CancellationTokenSource();


        object lid = new object();
        string lastSearchTerm = "";
        private async Task ExecuteSearchArticlesCommand()
        {
            try
            {
                var searchTerm = SearchText.Trim();
                if (searchTerm == LastSearchTerm)
                    return;
                LastSearchTerm = searchTerm;

                cts?.Cancel();
                lock (lid)
                {
                    Articles = new ObservableRangeCollection<Article>();
                }

                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    CurrentState = ListState.None;
                    return;
                }

                cts = new CancellationTokenSource();

                if (SearchText?.Length > 1)
                {
                    CurrentState = ListState.Loading;
                    Debug.WriteLine($">> Starting Search for {searchTerm}");

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

                    var articlesAsync = dataStore.GetArticleFromSearchAsync(searchTerm, cts.Token);
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
                        //Articles.AddRange(resultBucket);
                    }

                    if (Articles.Count == 0)
                        CurrentState = ListState.Empty; // you found nada
                    else
                        CurrentState = ListState.None;
                    timer.Stop();

                }
                else
                {
                    CurrentState = ListState.None; // go to collectionview empty state
                }
            }
            catch (OperationCanceledException oex)
            {
                Debug.WriteLine($"Cancelled");
            }
            catch (Exception ex)
            {
                CurrentState = ListState.Error;
            }
            finally
            {
                //IsBusy = false;
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
