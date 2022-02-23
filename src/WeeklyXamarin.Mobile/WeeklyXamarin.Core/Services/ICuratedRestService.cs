using System.Threading.Tasks;
using WeeklyXamarin.Core.Models;

namespace WeeklyXamarin.Core.Services
{
    public interface ICuratedRestService
    {
        string ApiKey { get; set; }
        string PublicationId { get; set; }

        Task<string> PostArticleToCurated(Article article);
    }
}