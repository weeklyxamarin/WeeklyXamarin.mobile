using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        Task<List<T>> SearchAsync(FormattableString filter);
        Task<List<T>> SearchAsync(Expression<Func<T, bool>> filter);
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
            var article = await tableClient.GetEntityAsync<T>("PartitionKey", id);
            return article;
        }

        public async Task<T> SaveAsync(T item)
        {
            await tableClient.UpsertEntityAsync(item);
            return item;
        }

        public async Task<List<T>> SearchAsync(FormattableString filter)
        {
            var pageable = tableClient.QueryAsync<T>(filter: TableClient.CreateQueryFilter(filter));

            var items = new List<T>();
            await foreach(var item in pageable)
            {
                items.Add(item);
            }
            return items;
        }

        public async Task<List<T>> SearchAsync(Expression<Func<T,bool>> filter)
        {
            var pageable = tableClient.QueryAsync<T>(filter);

            var items = new List<T>();
            await foreach (var item in pageable)
            {
                items.Add(item);
            }
            return items;
        }
    }
}
