using System.Threading.Tasks;
using Edelstein.Protocol.Datastore;
using Marten;

namespace Edelstein.Common.Datastore.Marten
{
    public class PostgresDataSessionBatch : IDataSessionBatch
    {
        private readonly IDocumentSession _session;

        public PostgresDataSessionBatch(IDocumentSession session)
            => _session = session;

        public IDataSessionBatch NewBatch()
            => new PostgresDataSessionBatch(_session);

        public IDataQuery<T> Query<T>() where T : class, IDataDocument
            => new PostgresDataQuery<T>(_session.Query<T>());

        public Task<T> Retrieve<T>(int id) where T : class, IDataDocument
            => Task.FromResult(_session.Load<T>(id));

        public Task Insert<T>(T entity) where T : class, IDataDocument
        {
            _session.Insert(entity);
            return Task.CompletedTask;
        }

        public Task Update<T>(T entity) where T : class, IDataDocument
        {
            _session.Update(entity);
            return Task.CompletedTask;
        }

        public Task Delete<T>(T entity) where T : class, IDataDocument
        {
            _session.Delete(entity);
            return Task.CompletedTask;
        }

        public Task Delete<T>(int id) where T : class, IDataDocument
        {
            _session.Delete<T>(id);
            return Task.CompletedTask;
        }

        public Task Discard()
        {
            _session.Dispose();
            return Task.CompletedTask;
        }

        public Task Save()
        {
            _session.SaveChanges();
            return Task.CompletedTask;
        }

        public void Dispose()
            => _session.Dispose();
    }
}
