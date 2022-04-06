using System.Collections.Generic;
using System.Threading.Tasks;
using WeeklyXamarin.AdminServices.Entities;
using WeeklyXamarin.Core.Models;

namespace WeeklyXamarin.AdminServices.Services
{
    public interface IAuthorStorage
    {
        Task<Author> PostAuthor(Author author);
        Task<List<AuthorEntity>> GetAuthors();
        Task<AuthorEntity> GetAuthor(string id);
        Task<bool> DeleteAuthor(string id);
    }
}
