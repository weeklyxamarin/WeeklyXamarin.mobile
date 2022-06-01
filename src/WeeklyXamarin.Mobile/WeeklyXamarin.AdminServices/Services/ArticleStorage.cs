using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ArticleStorage : IArticleStorage
    {

        private ITableService<Entities.ArticleEntity> tableService;

        public ArticleStorage(ITableService<Entities.ArticleEntity> tableService)
        {
            this.tableService = tableService;
        }

        public async Task<bool> DeleteArticle(string id)
        {
            return await tableService.DeleteAsync(id);
        }

        public async Task<Entities.ArticleEntity> GetArticle(string id)
        {
            var article = await tableService.GetAsync(id);
            return article;
        }

        public async Task<List<Entities.ArticleEntity>> GetArticles()
        {
            // TODO: Get the list of articles from table storage
            var articles = await tableService.GetAllAsync();
            return articles;
        }

        public async Task<ArticleEntity> PostArticle(ArticleEntity article)
        {
            Entities.ArticleEntity entity = article as Entities.ArticleEntity;
            entity.IsProcessed = true;
            return await tableService.SaveAsync(entity);
        }

        public async Task<List<Entities.ArticleEntity>> SearchArticle(string url)
        {
            //var articles = await tableService.SearchAsync($"Url eq {url}");

            var articles = await tableService.SearchAsync(x => x.Url == url);

            return articles;
        }
    }
}
