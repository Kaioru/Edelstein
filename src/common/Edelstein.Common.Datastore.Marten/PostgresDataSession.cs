using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Protocol.Datastore;
using Marten;

namespace Edelstein.Common.Datastore.Marten
{
    public class PostgresDataSession : IDataSession
    {
        private readonly IDocumentSession _session;

        public PostgresDataSession(IDocumentSession session)
            => _session = session;

        public IDataSessionBatch NewBatch()
            => new PostgresDataSessionBatch(_session);

        public IDataQuery<T> Query<T>() where T : class, IDataDocument
            => new PostgresDataQuery<T>(_session.Query<T>());

        public Task<T> Retrieve<T>(int id) where T : class, IDataDocument
            => _session.LoadAsync<T>(id);

        public Task Insert<T>(T entity) where T : class, IDataDocument
        {
            using var batch = NewBatch();

            batch.Insert(entity);
            return batch.Save();
        }

        public Task Update<T>(T entity) where T : class, IDataDocument
        {
            using var batch = NewBatch();

            batch.Update(entity);
            return batch.Save();
        }

        public Task Delete<T>(T entity) where T : class, IDataDocument
        {
            using var batch = NewBatch();

            batch.Delete(entity);
            return batch.Save();
        }

        public Task Delete<T>(int id) where T : class, IDataDocument
        {
            using var batch = NewBatch();

            batch.Delete<T>(id);
            return batch.Save();
        }

        public async Task<IEnumerable<T>> Retrieve<T>(IEnumerable<int> id) where T : class, IDataDocument
            => await _session.LoadManyAsync<T>(id);

        public Task Insert<T>(IEnumerable<T> entity) where T : class, IDataDocument
        {
            using var batch = NewBatch();

            batch.Insert(entity);
            return batch.Save();
        }

        public Task Update<T>(IEnumerable<T> entity) where T : class, IDataDocument
        {
            using var batch = NewBatch();

            batch.Update(entity);
            return batch.Save();
        }

        public Task Delete<T>(IEnumerable<T> entity) where T : class, IDataDocument
        {
            using var batch = NewBatch();

            batch.Delete(entity);
            return batch.Save();
        }

        public Task Delete<T>(IEnumerable<int> id) where T : class, IDataDocument
        {
            using var batch = NewBatch();

            batch.Delete<T>(id);
            return batch.Save();
        }

        public void Dispose()
            => _session.Dispose();
    }
}
