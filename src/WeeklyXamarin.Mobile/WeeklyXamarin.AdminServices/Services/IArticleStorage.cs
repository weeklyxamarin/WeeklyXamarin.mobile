using Azure.Data.Tables;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeeklyXamarin.AdminServices.Entities;
using WeeklyXamarin.Core.Models;

namespace WeeklyXamarin.AdminServices.Services
{
    public interface IArticleStorage
    {
        Task<Article> PostArticle(Article article);
        Task<List<ArticleEntity>> GetArticles();
        Task<ArticleEntity> GetArticle(string id);
        Task<bool> DeleteArticle(string id);
        Task<List<ArticleEntity>> SearchArticle(string url);
    }
}
