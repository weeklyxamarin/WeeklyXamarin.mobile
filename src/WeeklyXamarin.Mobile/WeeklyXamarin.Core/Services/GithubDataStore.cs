using MonkeyCache;
using MonkeyCache.FileStore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Models;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;
using Microsoft.Extensions.Logging;
using WeeklyXamarin.Core.Helpers;
using Index = WeeklyXamarin.Core.Models.Index;
using System.Runtime.CompilerServices;
using System.Threading;
using WeeklyXamarin.Core.Responses;

namespace WeeklyXamarin.Core.Services
{
    public class GithubDataStore : IDataStore
    {
        private readonly HttpClient _httpClient;
        private readonly IConnectivity _connectivity;
        private readonly IBarrel _barrel;
        private readonly ILogger<GithubDataStore> _logger;
        private readonly IAnalytics _analytics;

        const string baseUrl = @"https://raw.githubusercontent.com/weeklyxamarin/WeeklyXamarin.content/master/content/";
        const string indexFile = "index.json";
        const string tagsFile = "categories.json";
        const string authorsFile = "authors.json";
        const string acknowledgementsFile = "acknowledgements.json";

        public GithubDataStore(HttpClient httpClient, IConnectivity connectivity, IBarrel barrel, ILogger<GithubDataStore> logger, IAnalytics analytics)
        {
            _httpClient = httpClient;
            _connectivity = connectivity;
            _barrel = barrel;
            _logger = logger;
            _analytics = analytics;

            httpClient.BaseAddress = new Uri(baseUrl);
        }

        public void CheckEditionForSavedArticles(Edition edition)
        {
            var saved = GetSavedArticles(false);
            foreach(var article in edition.Articles)
            {
                article.IsSaved = saved.Articles.FirstOrDefault(x => x.Id == article.Id) != null;
            }
        }

        public async Task<Edition> GetEditionAsync(string id, bool forceRefresh = false)
        {
            //_logger.Log(LogLevel.Information, $"Getting Edition {id}");
            var editionFile = $"{id}.json";
            var edition = _barrel.Get<Edition>(key: editionFile);

            var cacheInvalid = !await CachedEditionUpToDate(edition);
            
            if ((cacheInvalid || forceRefresh) && 
                _connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    //_logger.Log(LogLevel.Information, $"  Getting {id} from the web");
                    // get from the interwebs
                    var editionResponse = await _httpClient.GetStringAsync(editionFile);

                    edition = JsonConvert.DeserializeObject<Edition>(editionResponse);

                    _barrel.Add(key: editionFile, data: edition, expireIn: TimeSpan.FromDays(999));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, nameof(GetEditionsAsync));
                    _analytics.TrackError(ex, new Dictionary<string, string> { { Constants.Analytics.Properties.FileName, editionFile } });
                }
            }

            if (edition != null) // smelly??!?!
                CheckEditionForSavedArticles(edition);

            return edition;
        }

        private async Task<bool> CachedEditionUpToDate(Edition cachedEdition)
        {
            if (cachedEdition == null) return false;

            var index = await GetEditionsAsync();
            var indexEdition = index.FirstOrDefault(edition => edition.Id == cachedEdition.Id);
            return (cachedEdition.UpdatedTimeStamp == indexEdition?.UpdatedTimeStamp);
        }

        public async Task<bool> PreloadNextEdition()
        {
            var index = _barrel.Get<Index>(key: indexFile);

            if (index is null)
                return false;
            
            var lastCachedEdition = index.Editions.FirstOrDefault();
            var allEditions = await GetEditionsAsync(true);
            var hasNew = lastCachedEdition.Id != allEditions.FirstOrDefault().Id;
            return hasNew;
        }



        public async Task<IEnumerable<Edition>> GetEditionsAsync(bool forceRefresh = false)
        {
            try
            {
                var index = await GetIndexAsync(forceRefresh);
                return index?.Editions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetEditionsAsync));
                _analytics.TrackError(ex, new Dictionary<string, string> { { Constants.Analytics.Properties.FileName, indexFile } });

                return null;
            }
        }

        private async Task<Index> GetIndexAsync(bool forceRefresh)
        {
            // try and get the index from cache
            var index = _barrel.Get<Index>(key: indexFile);

            // if we need to get it from the internet
            if (forceRefresh || index is null || index.IsStale)
            {
                // if we have internet
                if (_connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    try
                    {
                        // get it and cache it
                        var indexResponse = await _httpClient.GetStringAsync(indexFile);
                        index = JsonConvert.DeserializeObject<Index>(indexResponse);
                        index.FetchedDate = DateTime.UtcNow;
                        _barrel.Add(key: indexFile, data: index, expireIn: TimeSpan.FromMinutes(5));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to get index");
                    }
                }
            }
            return index;
        }

        public async Task<Article> GetArticleAsync(string editionId, string articleId)
        {
            var edition = await GetEditionAsync(editionId);
            var article = edition.Articles.FirstOrDefault(x => x.Id == articleId);
            return article;
        }

        public SavedArticleThing GetSavedArticles(bool forceRefresh)
        {
            // pull the articles out of monkeycache
            SavedArticleThing savedArticleList = _barrel.Get<SavedArticleThing>(key: Constants.BarrelNames.SavedArticles);
            if (savedArticleList == null)
                savedArticleList = new SavedArticleThing();

            return savedArticleList;
        }

        public void BookmarkArticle(Article articleToSave)
        {
            var savedArticleList = GetSavedArticles(false);
            articleToSave.IsSaved = true;
            savedArticleList.Add(articleToSave);

            _barrel.Add(key: Constants.BarrelNames.SavedArticles, data: savedArticleList, expireIn: TimeSpan.FromDays(999));
        }
        public void UnbookmarkArticle(Article articleToRemove)
        {
            var savedArticleList = GetSavedArticles(false);
            savedArticleList.Remove(articleToRemove);
            articleToRemove.IsSaved = false;

            // update the barrel
            _barrel.Add(key: Constants.BarrelNames.SavedArticles, data: savedArticleList, expireIn: TimeSpan.FromDays(999));
        }

        public async IAsyncEnumerable<Article> GetArticleFromSearchAsync(string searchText, string category, [EnumeratorCancellation]CancellationToken token, bool forceRefresh = false)
        {
            var editions = await GetEditionsAsync(forceRefresh);

            foreach (var edition in editions)
            {
                var articles = await GetEditionAsync(edition.Id, forceRefresh);

                foreach (var article in articles.Articles)
                {
                    if (article.Matches(searchText) && article.MatchesCategory(category))
                    {
                        token.ThrowIfCancellationRequested();
                        yield return article;
                    }
                }
            }
        }


        public async IAsyncEnumerable<Article> GetArticleForAuthorAsync(string author, [EnumeratorCancellation] CancellationToken token, bool forceRefresh = false)
        {
            var editions = await GetEditionsAsync(forceRefresh);

            foreach (var edition in editions)
            {
                var articles = await GetEditionAsync(edition.Id, forceRefresh);

                foreach (var article in articles.Articles)
                {
                    if (string.Equals(article.Author, author, StringComparison.InvariantCultureIgnoreCase))
                    {
                        token.ThrowIfCancellationRequested();
                        yield return article;
                    }
                }
            }
        }

        public async Task<IEnumerable<Category>> GetCategories(bool forceRefresh = false)
        {
            // try and get the index from cache
            var categoryResponse = _barrel.Get<CategoryResponse>(key: tagsFile);

            // if we need to get it from the internet
            if (forceRefresh || categoryResponse is null || categoryResponse.IsStale)
            {
                // if we have internet
                if (_connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    try
                    {
                        // get it and cache it
                        var responseText = await _httpClient.GetStringAsync(tagsFile);
                        categoryResponse = JsonConvert.DeserializeObject<CategoryResponse>(responseText);
                        categoryResponse.FetchedDate = DateTime.UtcNow;
                        _barrel.Add(key: tagsFile, data: categoryResponse, expireIn: TimeSpan.FromMinutes(5));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to get categories");
                    }
                }
            }

            return categoryResponse.Categories;
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync(bool forceRefresh = false)
        {
            // try and get the index from cache
            var authorResponse = _barrel.Get<AuthorResponse>(key: authorsFile);

            // if we need to get it from the internet
            if (forceRefresh || authorResponse is null || authorResponse.IsStale)
            {
                // if we have internet
                if (_connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    try
                    {
                        // get it and cache it
                        var responseText = await _httpClient.GetStringAsync(authorsFile);
                        authorResponse = JsonConvert.DeserializeObject<AuthorResponse>(responseText);
                        authorResponse.FetchedDate = DateTime.UtcNow;
                        _barrel.Add(key: authorsFile, data: authorResponse, expireIn: TimeSpan.FromMinutes(5));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to get authors");
                    }
                }
            }

            return authorResponse.Authors;
        }

        public async Task<Author> GetAuthorAsync(string id)
        {
            var authors = await GetAuthorsAsync();
            var author = authors.FirstOrDefault(x => x.Id == id);
            return author;
        }

        public async Task<Author> SearchAuthorsAsync(string authorName)
        {
            var authors = await GetAuthorsAsync();
            var author = authors.FirstOrDefault(x => x.Name == authorName);
            return author;
        }

        public async Task<List<Acknowledgement>> GetAcknowledgementsAsync(bool forceRefresh = false)
        {
            // try and get the index from cache
            var acknowledgementResponse = _barrel.Get<Response<List<Acknowledgement>>>(key: acknowledgementsFile);

            // if we need to get it from the internet
            if (forceRefresh || acknowledgementResponse is null || acknowledgementResponse.IsStale)
            {
                // if we have internet
                if (_connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    try
                    {
                        // get it and cache it
                        var responseText = await _httpClient.GetStringAsync(acknowledgementsFile);
                        acknowledgementResponse = JsonConvert.DeserializeObject<Response<List<Acknowledgement>>>(responseText);
                        acknowledgementResponse.FetchedDate = DateTime.UtcNow;
                        _barrel.Add(key: acknowledgementsFile, data: acknowledgementResponse, expireIn: TimeSpan.FromMinutes(5));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to get acknowledgements");
                    }
                }
            }

            return acknowledgementResponse.Data;
        }
    }
}
