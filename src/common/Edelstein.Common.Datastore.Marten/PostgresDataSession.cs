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

        public async Task<bool> Exists<T>(int id) where T : class, IDataDocument
             => (await Query<T>().Where(o => o.ID == id).Count()) > 0;

        public Task<T> Retrieve<T>(int id) where T : class, IDataDocument
            => Task.FromResult(_session.Load<T>(id));

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

        public void Dispose()
            => _session.Dispose();
    }
}
