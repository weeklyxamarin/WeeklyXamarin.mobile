using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Models.Api;

namespace WeeklyXamarin.Core.Services
{
    public class CuratedRestService : ICuratedRestService
    {
        private readonly HttpClient httpClient;
        private const string baseUrl = "https://localhost:5001/api";

        public string PublicationId { get; set; }
        public string ApiKey { get; set; }

        public CuratedRestService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task PostArticleToCurated(Article article)
        {
            var url = $"{baseUrl}/curated";
            var requestBody = article;
            
            var response = await httpClient.PostAsJsonAsync<Article>(url, requestBody);
        }
    }
}
