using System.Collections.Generic;
using System.Linq;
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
            => _session.LoadAsync<T>(id);

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

        public async Task<IEnumerable<T>> Retrieve<T>(IEnumerable<int> id) where T : class, IDataDocument
            => await _session.LoadManyAsync<T>(id);

        public Task Insert<T>(IEnumerable<T> entity) where T : class, IDataDocument
        {
            _session.Insert(entity);
            return Task.CompletedTask;
        }

        public Task Update<T>(IEnumerable<T> entity) where T : class, IDataDocument
        {
            _session.Update(entity);
            return Task.CompletedTask;
        }

        public Task Delete<T>(IEnumerable<T> entity) where T : class, IDataDocument
        {
            _session.Delete(entity);
            return Task.CompletedTask;
        }

        public Task Delete<T>(IEnumerable<int> id) where T : class, IDataDocument
            => Task.WhenAll(id.Select(i => Delete<T>(i)));

        public Task Discard()
        {
            _session.Dispose();
            return Task.CompletedTask;
        }

        public Task Save()
            => _session.SaveChangesAsync();

        public void Dispose()
            => _session.Dispose();
    }
}
