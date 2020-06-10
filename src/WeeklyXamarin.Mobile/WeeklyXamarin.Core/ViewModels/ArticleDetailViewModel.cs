using System;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Services;

namespace WeeklyXamarin.Core.ViewModels
{
    public class ArticleDetailViewModel : ViewModelBase
    {
        private readonly IDataStore dataStore;
        private Article article;

        public Article Article {
            get => article;
            set => SetProperty(ref article, value);
        }
        public ArticleDetailViewModel(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public async Task Initialize(string articleId)
        {
            var article = await dataStore.GetArticleAsync(articleId);

            Title = article.Title;
            Article = article;
        }

    }
}
