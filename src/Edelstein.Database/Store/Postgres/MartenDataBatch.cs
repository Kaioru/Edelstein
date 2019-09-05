using System.Threading.Tasks;
using Marten;

namespace Edelstein.Database.Store.Postgres
{
    public class MartenDataBatch : IDataBatch
    {
        private readonly IDocumentSession _session;

        public MartenDataBatch(IDocumentSession session)
            => _session = session;

        public void Insert<T>(T entity) where T : class, IDataEntity
            => _session.Insert<T>(entity);

        public void Update<T>(T entity) where T : class, IDataEntity
            => _session.Update<T>(entity);

        public void Delete<T>(T entity) where T : class, IDataEntity
            => _session.Delete<T>(entity);

        public void SaveChanges()
            => _session.SaveChanges();

        public Task SaveChangesAsync()
            => _session.SaveChangesAsync();

        public void Dispose()
        {
            // Do nothing
        }
    }
}