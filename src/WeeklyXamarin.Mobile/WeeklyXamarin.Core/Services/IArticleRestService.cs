
using System.Collections.Generic;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Models;

namespace WeeklyXamarin.Core.Services
{
    public interface IArticleRestService
    {
        Task<Article> GetArticleDetailsFromUrl(string articleUrl);
        Task<Article> PostArticle(Article article);
        Task<List<Article>> GetDraftArticles();
        Task<Article> GetArticle(string id);
        Task<bool> DeleteArticle(string id);
    }
}