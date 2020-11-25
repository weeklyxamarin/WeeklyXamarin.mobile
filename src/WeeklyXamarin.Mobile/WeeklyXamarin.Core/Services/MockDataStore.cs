using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Models;

namespace WeeklyXamarin.Core.Services
{
    public class MockDataStore //: IDataStore
    {
        //readonly List<Item> items;
        readonly IList<Edition> editions;
        readonly IList<Article> articles;

        Random rnd = new Random();

        public MockDataStore()
        {
            // create mock list of editions and articles
            editions = new ObservableCollection<Edition>();
            articles = new ObservableCollection<Article>();

            for (int i = 0; i < 257; i++)
            {
                var newEdition = new Edition()
                {
                    Id = i.ToString(),
                    Summary = "Bacon ipsum dolor amet pastrami kielbasa jowl pork, tongue tri-tip picanha sausage pork loin andouille buffalo frankfurter brisket alcatra. Pancetta sausage chislic picanha alcatra filet mignon tri-tip, pig buffalo beef rump short ribs pork belly.",
                    PublishDate = DateTime.Now.AddDays(-7 * i),
                    Curators = "Luce and Kym"
                };
                editions.Add(newEdition);

                var numArticles = rnd.Next(15, 25);
                for (int a = 0; a < numArticles; a++)
                {
                    var mockArticle = new Article()
                    {
                        Id = a.ToString(),
                        EditionId = newEdition.Id,
                        Url = $"http://www.microsoft.com",
                        Title = $"Article Title {a}",
                        Description = $"Article description {a}",
                        Author = "Jill Bloggs",
                        Category = "Xamarin.Forms"
                    };
                    articles.Add(mockArticle);
                }
            }
        }

        public async Task<Edition> GetEditionAsync(string id)
        {
            return await Task.FromResult(editions.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Edition>> GetEditionsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(editions);
        }


        public async Task<IEnumerable<Article>> GetArticlesForEditionAsync(string editionId, bool forceRefresh = false)
        {
            return await Task.FromResult(articles.Where(e => e.EditionId == editionId));
        }

        public Task<Edition> GetEditionAsync(string id, bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<Article> GetArticleAsync(string editionId, string articleId)
        {
            return Task.FromResult(articles.FirstOrDefault(s => s.Id == articleId));
        }

        public void BookmarkArticle(Article articleToSave)
        {
            throw new NotImplementedException();
        }

        public void UnbookmarkArticle(Article articleToRemove)
        {
            throw new NotImplementedException();
        }

        //SavedArticleThing IDataStore.GetSavedArticles(bool forceRefresh)
        //{
        //    throw new NotImplementedException();
        //}

        public IAsyncEnumerable<Article> GetArticleFromSearchAsync(string searchText, bool forceRefresh)
        {
            throw new NotImplementedException();
        }
    }
}