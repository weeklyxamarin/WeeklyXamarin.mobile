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

    public class AuthorRestService : IAuthorRestService
    {
        private readonly HttpClient httpClient;
        private const string baseUrl = "/api";


        public AuthorRestService(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient(Constants.HttpClientKeys.WeeklyXamarin);
        }

        public async Task<Author> PostAuthor(Author Author)
        {
            var url = $"{baseUrl}/Author";

            var response = await httpClient.PostAsJsonAsync<Author>(url, Author);

            var returnAuthor = await response.Content.ReadFromJsonAsync<Author>();
            return returnAuthor;
        }

        public async Task<List<Author>> GetAllAuthors()
        {
            var url = $"{baseUrl}/Author";

            var Authors = await httpClient.GetFromJsonAsync<List<Author>>(url);
            return Authors;
        }

        public async Task<Author> GetAuthor(string id)
        {
            var url = $"{baseUrl}/Author/{id}";

            var Author = await httpClient.GetFromJsonAsync<Author>(url);
            return Author;
        }

        public async Task<bool> DeleteAuthor(string id)
        {
            var url = $"{baseUrl}/Author/{id}";

            var response = await httpClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;

        }
    }
}
