using System.Threading.Tasks;
using Marten;

namespace Edelstein.Database.Postgres
{
    public class MartenDataBatch : IDataBatch
    {
        private readonly IDocumentSession _session;

        public MartenDataBatch(IDocumentSession session)
            => _session = session;

        public T Retrieve<T>(int id) where T : class, IDataEntity
            => _session.Load<T>(id);

        public void Insert<T>(T entity) where T : class, IDataEntity
            => _session.Insert<T>(entity);

        public void Update<T>(T entity) where T : class, IDataEntity
            => _session.Update<T>(entity);

        public void Delete<T>(T entity) where T : class, IDataEntity
            => _session.Delete<T>(entity);

        public Task<T> RetrieveAsync<T>(int id) where T : class, IDataEntity
            => _session.LoadAsync<T>(id);

        public Task InsertAsync<T>(T entity) where T : class, IDataEntity
            => Task.Run(() => Insert<T>(entity));

        public Task UpdateAsync<T>(T entity) where T : class, IDataEntity
            => Task.Run(() => Update<T>(entity));

        public Task DeleteAsync<T>(T entity) where T : class, IDataEntity
            => Task.Run(() => Delete<T>(entity));


        public void Dispose()
            => _session?.Dispose();

        public void SaveChanges()
            => _session.SaveChanges();

        public Task SaveChangesAsync()
            => _session.SaveChangesAsync();
    }
}