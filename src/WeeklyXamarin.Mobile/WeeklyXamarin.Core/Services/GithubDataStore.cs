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

namespace WeeklyXamarin.Core.Services
{
    public class GithubDataStore : IDataStore
    {
        readonly HttpClient _httpClient;
        private IConnectivity _connectivity;
        const string baseUrl = @"https://raw.githubusercontent.com/weeklyxamarin/WeeklyXamarin.content/master/content/";
        const string indexFile = "index.json";

        public GithubDataStore(HttpClient httpClient, IConnectivity connectivity)
        {
            _httpClient = httpClient;
            httpClient.BaseAddress = new Uri(baseUrl);

            _connectivity = connectivity;
        }

        public async Task<Edition> GetEditionAsync(string id, bool forceRefresh = false)
        {
            var editionFile = $"{id}.json";
            var edition = Barrel.Current.Get<Edition>(key: editionFile);

            if (await CachedEditionUpToDate(edition) && !forceRefresh)
                return edition;

            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                return edition; 
            }

            try
            {
                var editionResponse = await _httpClient.GetStringAsync(editionFile);

                edition = JsonConvert.DeserializeObject<Edition>(editionResponse);

                Barrel.Current.Add(key: editionFile, data: edition, expireIn: TimeSpan.FromDays(999));
                return edition;
            }
            catch (Exception ex)
            {
                //TODO Add a logging framework
                Debug.WriteLine($"error getting edition \n{ex}\n{ex.StackTrace}");
                return edition;
            }
        }

        private async Task<bool> CachedEditionUpToDate(Edition cachedEdition)
        {
            if (cachedEdition == null) return false;

            var index = await GetEditionsAsync();
            var indexEdition = index.FirstOrDefault(edition => edition.Id == cachedEdition.Id);
            return (cachedEdition.UpdatedTimeStamp == indexEdition?.UpdatedTimeStamp);
        }

        public async Task<IEnumerable<Edition>> GetEditionsAsync(bool forceRefresh = false)
        {
            var index = Barrel.Current.Get<Index>(key: indexFile);

            if (_connectivity.NetworkAccess != NetworkAccess.Internet ||
                 (index?.FetchedDate > DateTime.UtcNow.AddMinutes(-5) &&
                 forceRefresh == false))
            {
                return index?.Editions;
            }

            try
            {
                var indexResponse = await _httpClient.GetStringAsync(indexFile);

                index = JsonConvert.DeserializeObject<Index>(indexResponse);
                index.FetchedDate = DateTime.UtcNow;
                Barrel.Current.Add(key: indexFile, data: index, expireIn: TimeSpan.FromMinutes(5));

                return index.Editions;
            }
            catch (Exception ex)
            {
                //TODO Add a logging framework
                Debug.WriteLine($"error getting index \n{ex}\n{ex.StackTrace}");
                return index?.Editions;
            }
        }

        public async Task<Article> GetArticleAsync(string editionId, string articleId)
        {
            var edition = await GetEditionAsync(editionId);
            var article = edition.Articles.FirstOrDefault(x => x.Id == articleId);
            return article;
        }

        public Task<IEnumerable<Article>> GetArticlesForEditionAsync(string editionId, bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }
    }
}
