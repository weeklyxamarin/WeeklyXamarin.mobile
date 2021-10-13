using System;
using System.Collections.Generic;
using System.Threading;
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
        void BookmarkArticle(Article articleToSave);
        void UnbookmarkArticle(Article articleToRemove);
        IAsyncEnumerable<Article> GetArticleFromSearchAsync(string searchText, string category, CancellationToken token, bool forceRefresh = false);
        Task<bool> PreloadNextEdition();
        Task<IEnumerable<Category>> GetCategories(bool forceRefresh = false);
        Task<Author> GetAuthorAsync(string id);
        Task<IEnumerable<Author>> GetAuthorsAsync(bool forceRefresh = false);
        Task<Author> SearchAuthorsAsync(string authorName);
    }
}
