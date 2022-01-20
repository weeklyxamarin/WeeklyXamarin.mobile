using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Services;

namespace WeeklyXamarin.AdminServices.Services
{
    public class UrlService : IUrlService
    {
        private IDataStore _dataStore;

        public UrlService(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public async Task<Article> GetArticleDetailsFromUrl(string url)
        {

            var editions = await _dataStore.GetEditionsAsync();
            var edition = await _dataStore.GetEditionAsync(editions.FirstOrDefault().Id);

            var article = edition.Articles.FirstOrDefault();
            return article;
            //article.Url = url;
            //article.Title = "New Title";
            //article.Description = "Description";
            //return article;
        }
    }
}
