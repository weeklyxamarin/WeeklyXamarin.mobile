
using System.Collections.Generic;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Models;

namespace WeeklyXamarin.Core.Services
{
    public interface IAuthorRestService
    {
        Task<Author> PostAuthor(Author author);
        Task<Author> GetAuthor(string id);
        Task<List<Author>> GetAllAuthors();
        Task<bool> DeleteAuthor(string id);
    }
}
