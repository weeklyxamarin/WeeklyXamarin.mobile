using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Azure;
using Azure.Data.Tables;

namespace WeeklyXamarin.AdminServices.Services
{
    public class TableClient<T> : TableClient where T : class, ITableEntity, new()
    {
        public TableClient(string connectionString) : base(connectionString, typeof(T).Name)
        {
        }

        public override Response<T> GetEntity<T>(string partitionKey, string rowKey, IEnumerable<string> select = null, CancellationToken cancellationToken = default)
        {
            return base.GetEntity<T>(partitionKey, rowKey, select, cancellationToken);
        }

        public override Response UpsertEntity<T>(T entity, TableUpdateMode mode = TableUpdateMode.Merge, CancellationToken cancellationToken = default)
        {
            return base.UpsertEntity<T>(entity, mode, cancellationToken);
        }

        public override Pageable<T> Query<T>(string filter = null, int? maxPerPage = null, IEnumerable<string> select = null, CancellationToken cancellationToken = default)
        {
            return base.Query<T>(filter, maxPerPage, select, cancellationToken);
        }
    }
}
