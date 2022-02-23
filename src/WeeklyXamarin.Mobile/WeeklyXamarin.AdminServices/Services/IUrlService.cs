using System.Threading.Tasks;
using WeeklyXamarin.Core.Models;

namespace WeeklyXamarin.AdminServices.Services
{
    public interface IUrlService
    {
        Task<Article> GetArticleDetailsFromUrl(string url);
    }
}