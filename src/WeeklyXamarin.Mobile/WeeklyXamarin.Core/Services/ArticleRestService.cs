using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Models.Api;
using Microsoft.Extensions.Http;
using WeeklyXamarin.Core.Helpers;

namespace WeeklyXamarin.Core.Services
{

    public class ArticleRestService : IArticleRestService
    {
        private readonly HttpClient httpClient;
        private const string baseUrl = "/api";


        public ArticleRestService(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient(Constants.HttpClientKeys.WeeklyXamarin);
        }

        public async Task<Article> GetArticleDetailsFromUrl(string articleUrl)
        {
            var url = $"{baseUrl}/article/Lookup";
            var requestBody = new LookupUrlRequest { Url = articleUrl };

            var response = await httpClient.PostAsJsonAsync<LookupUrlRequest>(url, requestBody);

            var article = await response.Content.ReadFromJsonAsync<Article>();
            return article;

        }

        public async Task<Article> PostArticle(Article article)
        {
            var url = $"{baseUrl}/article";

            var response = await httpClient.PostAsJsonAsync<Article>(url, article);

            var returnArticle = await response.Content.ReadFromJsonAsync<Article>();
            return returnArticle;
        }

        public async Task<List<Article>> GetDraftArticles()
        {
            var url = $"{baseUrl}/article";

            var articles = await httpClient.GetFromJsonAsync<List<Article>>(url);
            return articles;
        }

        public async Task<Article> GetArticle(string id)
        {
            var url = $"{baseUrl}/article/{id}";

            var article = await httpClient.GetFromJsonAsync<Article>(url);
            return article;
        }

        public async Task<bool> DeleteArticle(string id)
        {
            var url = $"{baseUrl}/article/{id}";

            var response = await httpClient.DeleteAsync(url).ConfigureAwait(false);
            return response.IsSuccessStatusCode;

        }
    }
}
