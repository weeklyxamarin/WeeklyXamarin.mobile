using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeeklyXamarin.Mobile.Models;

namespace WeeklyXamarin.Mobile.Services
{
    public interface IDataStore
    {
        // Editions
        Task<Edition> GetEditionAsync(string id);
        Task<IEnumerable<Edition>> GetEditionsAsync(bool forceRefresh = false);

        // Articles
        Task<Article> GetArticleAsync(string id);
        Task<IEnumerable<Article>> GetArticlesForEditionAsync(string editionId, bool forceRefresh = false);
    }
}
