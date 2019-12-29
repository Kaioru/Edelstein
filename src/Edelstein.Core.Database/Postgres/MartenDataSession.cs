using System.Threading.Tasks;
using Edelstein.Database.Utils;
using Marten;

namespace Edelstein.Database.Postgres
{
    public class MartenDataSession : IDataSession
    {
        private readonly IDocumentSession _session;

        public MartenDataSession(IDocumentSession session)
            => _session = session;

        public IDataQuery<T> Query<T>() where T : class, IDataEntity
            => new QueryableDataQuery<T>(_session.Query<T>());

        public IDataBatch Batch()
            => new MartenDataBatch(_session);

        public T Retrieve<T>(int id) where T : class, IDataEntity
            => _session.Load<T>(id);

        public void Insert<T>(T entity) where T : class, IDataEntity
        {
            _session.Insert<T>(entity);
            _session.SaveChanges();
        }

        public void Update<T>(T entity) where T : class, IDataEntity
        {
            _session.Update<T>(entity);
            _session.SaveChanges();
        }

        public void Delete<T>(T entity) where T : class, IDataEntity
        {
            _session.Delete<T>(entity);
            _session.SaveChanges();
        }

        public Task<T> RetrieveAsync<T>(int id) where T : class, IDataEntity
            => _session.LoadAsync<T>(id);

        public Task InsertAsync<T>(T entity) where T : class, IDataEntity
        {
            _session.Insert<T>(entity);
            return _session.SaveChangesAsync();
        }

        public Task UpdateAsync<T>(T entity) where T : class, IDataEntity
        {
            _session.Update<T>(entity);
            return _session.SaveChangesAsync();
        }

        public Task DeleteAsync<T>(T entity) where T : class, IDataEntity
        {
            _session.Delete<T>(entity);
            return _session.SaveChangesAsync();
        }

        public void Dispose()
            => _session?.Dispose();
    }
}