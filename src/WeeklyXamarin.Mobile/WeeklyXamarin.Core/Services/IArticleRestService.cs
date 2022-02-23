using System.Threading.Tasks;
using WeeklyXamarin.Core.Models;

namespace WeeklyXamarin.Core.Services
{
    public interface IArticleRestService
    {
        Task<Article> GetArticleDetailsFromUrl(string articleUrl);
    }
}