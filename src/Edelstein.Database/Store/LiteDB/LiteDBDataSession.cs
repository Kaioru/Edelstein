using System.Linq;
using System.Threading.Tasks;
using LiteDB;

namespace Edelstein.Database.Store.LiteDB
{
    public class LiteDBDataSession : IDataSession
    {
        private readonly LiteDatabase _database;

        public LiteDBDataSession(LiteDatabase database)
        {
            _database = database;
        }

        public IDataQuery<T> Query<T>() where T : class, IDataEntity
            => new LiteDBDataQuery<T>(_database.GetCollection<T>().FindAll().AsQueryable());

        public IDataBatch Batch()
            => new LiteDBDataBatch(_database);

        public void Insert<T>(T entity) where T : class, IDataEntity
            => _database.GetCollection<T>().Insert(entity);

        public void Update<T>(T entity) where T : class, IDataEntity
            => _database.GetCollection<T>().Update(entity);

        public void Delete<T>(T entity) where T : class, IDataEntity
            => _database.GetCollection<T>().Delete(entity.ID);

        public Task InsertAsync<T>(T entity) where T : class, IDataEntity
            => Task.Run(() => Insert(entity));

        public Task UpdateAsync<T>(T entity) where T : class, IDataEntity
            => Task.Run(() => Update(entity));

        public Task DeleteAsync<T>(T entity) where T : class, IDataEntity
            => Task.Run(() => Delete(entity));

        public void Dispose()
            => _database.Dispose();
    }
}