using System;

using WeeklyXamarin.Mobile.Models;

namespace WeeklyXamarin.Mobile.ViewModels
{
    public class ArticleDetailViewModel : BaseViewModel
    {
        public Article Article { get; set; }
        public ArticleDetailViewModel(Article article = null)
        {
            Title = article?.Title;
            Article = article;
        }
    }
}
