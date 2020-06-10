using System;
using WeeklyXamarin.Core.Models;

namespace WeeklyXamarin.Core.ViewModels
{
    public class ArticleDetailViewModel : ViewModelBase
    {
        public Article Article { get; set; }
        public ArticleDetailViewModel(Article article = null)
        {
            Title = article?.Title;
            Article = article;
        }
    }
}
