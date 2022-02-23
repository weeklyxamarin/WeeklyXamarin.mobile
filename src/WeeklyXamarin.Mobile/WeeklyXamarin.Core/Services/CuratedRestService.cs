using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Helpers;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Models.Api;

namespace WeeklyXamarin.Core.Services
{
    public class CuratedRestService : ICuratedRestService
    {
        private readonly HttpClient httpClient;
        private const string baseUrl = "/api";

        public string PublicationId { get; set; }
        public string ApiKey { get; set; }

        public CuratedRestService(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient(Constants.HttpClientKeys.WeeklyXamarin);
        }

        public async Task<string> PostArticleToCurated(Article article)
        {
            var url = $"{baseUrl}/curated";
            var requestBody = article;
            
            var response = await httpClient.PostAsJsonAsync<Article>(url, requestBody);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}
