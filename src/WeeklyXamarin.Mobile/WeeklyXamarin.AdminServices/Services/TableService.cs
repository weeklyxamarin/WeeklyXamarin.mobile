using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeeklyXamarin.AdminServices.Services
{
    public interface ITableService<T> where T : class, ITableEntity, new()
    {
        Task<List<T>> GetAllAsync();
        Task<T> SaveAsync(T item);
        Task<T> GetAsync(string id);
        Task<bool> DeleteAsync(string id);
    }

    public class TableService<T> : ITableService<T> where T : class, ITableEntity, new()
    {
        private TableClient tableClient;
        

        public TableService(TableClient<T> tableClient)
        {
            this.tableClient = tableClient;
            tableClient.CreateIfNotExists();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var response = await tableClient.DeleteEntityAsync("Article", id);
            return !response.IsError;

        }

        public Task<List<T>> GetAllAsync()
        {
            //investigate async api
            var items = tableClient.Query<T>().ToList();
            return Task.FromResult(items);
        }

        public async Task<T> GetAsync(string id)
        {
            var article = await tableClient.GetEntityAsync<T>("Article", id);
            return article;
        }

        public async Task<T> SaveAsync(T item)
        {
            await tableClient.UpsertEntityAsync(item);
            return item;
        }
    }
}
