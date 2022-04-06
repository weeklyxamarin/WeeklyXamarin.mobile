using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeeklyXamarin.AdminServices.Entities;
using WeeklyXamarin.Core.Models;

namespace WeeklyXamarin.AdminServices.Services
{
    public class AuthorStorage : IAuthorStorage
    {
        private ITableService<AuthorEntity> tableService;

        public AuthorStorage(ITableService<AuthorEntity> tableService)
        {
            this.tableService = tableService;
        }

        public async Task<bool> DeleteAuthor(string id)
        {
            return await tableService.DeleteAsync(id);
        }

        public async Task<AuthorEntity> GetAuthor(string id)
        {
            var Author = await tableService.GetAsync(id);
            return Author;
        }

        public async Task<List<AuthorEntity>> GetAuthors()
        {
            // TODO: Get the list of Authors from table storage
            var Authors = await tableService.GetAllAsync();
            return Authors;
        }

        public async Task<Author> PostAuthor(Author Author)
        {
            AuthorEntity entity = Author as AuthorEntity;
            return await tableService.SaveAsync(entity);
        }
    }
}
