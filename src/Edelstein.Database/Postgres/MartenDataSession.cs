using System.Threading.Tasks;
using Marten;

namespace Edelstein.Database.Postgres
{
    public class MartenDataSession : IDataSession
    {
        private readonly IDocumentSession _session;

        public MartenDataSession(IDocumentSession session)
            => _session = session;

        public IDataQuery<T> Query<T>()
            => new MartenDataQuery<T>(_session.Query<T>());

        public T Load<T>(int id)
            => _session.Load<T>(id);

        public void Insert<T>(T entity)
        {
            _session.Insert<T>(entity);
            _session.SaveChanges();
        }

        public void Update<T>(T entity)
        {
            _session.Update<T>(entity);
            _session.SaveChanges();
        }

        public void Delete<T>(T entity)
        {
            _session.Delete<T>(entity);
            _session.SaveChanges();
        }

        public Task<T> LoadAsync<T>(int id)
            => _session.LoadAsync<T>(id);

        public Task InsertAsync<T>(T entity)
        {
            _session.Insert<T>(entity);
            return _session.SaveChangesAsync();
        }

        public Task UpdateAsync<T>(T entity)
        {
            _session.Update<T>(entity);
            return _session.SaveChangesAsync();
        }

        public Task DeleteAsync<T>(T entity)
        {
            _session.Delete<T>(entity);
            return _session.SaveChangesAsync();
        }

        public void Dispose()
            => _session?.Dispose();
    }
}