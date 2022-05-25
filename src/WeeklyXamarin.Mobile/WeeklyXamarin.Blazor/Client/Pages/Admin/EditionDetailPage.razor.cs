using System.Collections.Generic;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Models;

namespace WeeklyXamarin.Blazor.Client.Pages.Admin;

public partial class EditionDetailPage
{
    List<Article> SelectedArticles { get; set; } = new();
    async Task AddArticle(Article article)
    {
        SelectedArticles.Add(article);
    }
}
