using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Models;

namespace WeeklyXamarin.Core.Services
{
    public interface IDataStore
    {
        // Editions
        Task<Edition> GetEditionAsync(string id, bool forceRefresh = false);
        Task<IEnumerable<Edition>> GetEditionsAsync(bool forceRefresh = false);
        // Articles
        Task<Article> GetArticleAsync(string editionId, string articleId);
        SavedArticleThing GetSavedArticles(bool forceRefresh);
        void SaveArticle(Article articleToSave);
        void UnSaveArticle(Article articleToRemove);
    }
}
