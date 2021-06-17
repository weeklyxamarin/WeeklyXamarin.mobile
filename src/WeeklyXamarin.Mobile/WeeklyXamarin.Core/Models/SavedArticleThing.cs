using System;
using System.Collections.Generic;
using System.Linq;

namespace WeeklyXamarin.Core.Models
{
    public class SavedArticleThing
    {
        public List<Article> Articles { get; set; } = new List<Article>();

        public void Add(Article articleToSave)
        {
            var article = Articles.FirstOrDefault(a => a.Id == articleToSave.Id);
            if (article == null)
                Articles.Add(articleToSave);
        }

        public void Insert(int index, Article articleToSave)
        {
            var article = Articles.FirstOrDefault(a => a.Id == articleToSave.Id);
            if (article == null)
                Articles.Insert(index, articleToSave);
        }

        public void Remove(Article articleToRemove)
        {
            var article = Articles.FirstOrDefault(a => a.Id == articleToRemove.Id);
            if (article != null)
                Articles.Remove(article);
        }

    }
}
