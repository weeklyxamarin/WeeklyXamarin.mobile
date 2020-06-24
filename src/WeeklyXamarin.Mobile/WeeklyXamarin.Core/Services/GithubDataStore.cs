using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Models;

namespace WeeklyXamarin.Core.Services
{
    public class GithubDataStore : IDataStore
    {
        readonly HttpClient _httpClient;
        const string baseUrl = @"https://raw.githubusercontent.com/weeklyxamarin/WeeklyXamarin.content/master/content/";

        public GithubDataStore(HttpClient httpClient)
        {
            _httpClient = httpClient;
            httpClient.BaseAddress = new Uri(baseUrl);
        }

        public Task<Article> GetArticleAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Article>> GetArticlesForEditionAsync(string editionId, bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<Edition> GetEditionAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Edition>> GetEditionsAsync(bool forceRefresh = false)
        {
            var indexResponse = await  _httpClient.GetStringAsync($"index.json");
            var index = JsonConvert.DeserializeObject<Index>(indexResponse);
            return index.Editions;
        }
    }
}
