using System.Threading.Tasks;
using Marten;

namespace Edelstein.Database.Store.Postgres
{
    public class MartenDataSession : IDataSession
    {
        private readonly IDocumentSession _session;

        public MartenDataSession(IDocumentSession session)
            => _session = session;

        public IDataQuery<T> Query<T>() where T : class, IDataEntity
            => new MartenDataQuery<T>(_session.Query<T>());

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