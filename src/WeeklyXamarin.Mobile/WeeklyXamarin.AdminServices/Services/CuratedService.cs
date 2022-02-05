using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Models.Api;

namespace WeeklyXamarin.AdminServices.Services
{
    public class CuratedService : ICuratedService
    {

        HttpClient httpClient;
        private const string baseUrl = "https://api.curated.co/api/v3";

        public string ApiKey { get; set; } = default!;
        public string Subscription { get; set; } = default!;

        public CuratedService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<string> PostArticleToCurated(Article article)
        {
            // doo all the call stuff    
            var url = $"{baseUrl}/publications/{Subscription}/links"; 
            var requestBody = article;

            httpClient.DefaultRequestHeaders.Add("authorization", $"Token token=\"{ApiKey}\"");

            var response = await httpClient.PostAsJsonAsync<Article>(url, requestBody);
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(result);
            return result;

        }
    }
}
