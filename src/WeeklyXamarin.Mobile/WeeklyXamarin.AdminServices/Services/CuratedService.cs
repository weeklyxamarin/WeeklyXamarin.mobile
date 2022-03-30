using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WeeklyXamarin.AdminServices.Entities;
using WeeklyXamarin.Core.Helpers;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Models.Api;
using WeeklyXamarin.Core.Services;

namespace WeeklyXamarin.AdminServices.Services
{
    public class CuratedService : ICuratedService
    {

        HttpClient httpClient;
        private IDataStore dataStore;
        private ITableService<ArticleEntity> tableService;

        public string ApiKey { get; set; } = default!;
        public string Subscription { get; set; } = default!;

        public CuratedService(IHttpClientFactory httpClientFactory, IDataStore datastore)
        {
            this.httpClient = httpClientFactory.CreateClient(Constants.HttpClientKeys.Curated);
            this.dataStore = datastore;
        }

        public async Task<string> PostArticle(Article article)
        {
            if (article == null)
                throw new ArgumentNullException(nameof(article));

            // for curated we have a special format at the end of the article including the 
            // article link title and author
            Author author = await dataStore.SearchAuthorsAsync(article.Author);
            var curatedByLine = $"[**{article?.Title}**]({article?.Url}) by [{author?.Name}]({author?.PreferredContact})";

            article.Description = article?.Description + Environment.NewLine + Environment.NewLine + curatedByLine;

            var url = $"publications/{Subscription}/links";
            var requestBody = article;

            httpClient.DefaultRequestHeaders.Add("authorization", $"Token token=\"{ApiKey}\"");

            var response = await httpClient.PostAsJsonAsync<Article>(url, requestBody);
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(result);
            return result;
        }
    }
}
