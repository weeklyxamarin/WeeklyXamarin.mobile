using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Helpers;
using WeeklyXamarin.Core.Models;

namespace WeeklyXamarin.Core.Services
{
    public interface IEditionRestService
    {
        Task<bool> DeleteEdition(string id);
        Task<List<Edition>> GetAllEditions();
        Task<Edition> GetEdition(string id);
        Task<Edition> PostEdition(Edition Edition);
    }

    public class EditionRestService : IEditionRestService
    {
        private readonly HttpClient httpClient;
        private const string baseUrl = "/api";


        public EditionRestService(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient(Constants.HttpClientKeys.WeeklyXamarin);
        }

        public async Task<Edition> PostEdition(Edition Edition)
        {
            var url = $"{baseUrl}/Edition";

            var response = await httpClient.PostAsJsonAsync<Edition>(url, Edition);

            var returnEdition = await response.Content.ReadFromJsonAsync<Edition>();
            return returnEdition;
        }

        public async Task<List<Edition>> GetAllEditions()
        {
            var url = $"{baseUrl}/Edition";

            var Editions = await httpClient.GetFromJsonAsync<List<Edition>>(url);
            return Editions;
        }

        public async Task<Edition> GetEdition(string id)
        {
            var url = $"{baseUrl}/Edition/{id}";

            var Edition = await httpClient.GetFromJsonAsync<Edition>(url);
            return Edition;
        }

        public async Task<bool> DeleteEdition(string id)
        {
            var url = $"{baseUrl}/Edition/{id}";

            var response = await httpClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;

        }
    }

    
}
