using System.Threading.Tasks;
using Edelstein.Database.Utils;
using JsonFlatFileDataStore;

namespace Edelstein.Database.Json
{
    public class JsonDataSession : IDataSession
    {
        private readonly DataStore _dataStore;

        public JsonDataSession(DataStore dataStore)
            => _dataStore = dataStore;

        public IDataQuery<T> Query<T>() where T : class, IDataEntity
            => new EnumerableDataQuery<T>(_dataStore.GetCollection<T>().AsQueryable());

        public IDataBatch Batch()
            => new QueuedDataBatch(this);

        public void Insert<T>(T entity) where T : class, IDataEntity
        {
            entity.ID = _dataStore.GetCollection<T>().GetNextIdValue();
            _dataStore.GetCollection<T>().InsertOne(entity);
        }

        public void Update<T>(T entity) where T : class, IDataEntity
            => _dataStore.GetCollection<T>().UpdateOne(entity.ID, entity);

        public void Delete<T>(T entity) where T : class, IDataEntity
            => _dataStore.GetCollection<T>().DeleteOne(entity.ID);

        public Task InsertAsync<T>(T entity) where T : class, IDataEntity
        {
            entity.ID = _dataStore.GetCollection<T>().GetNextIdValue();
            return _dataStore.GetCollection<T>().InsertOneAsync(entity);
        }

        public Task UpdateAsync<T>(T entity) where T : class, IDataEntity
            => _dataStore.GetCollection<T>().UpdateOneAsync(entity.ID, entity);

        public Task DeleteAsync<T>(T entity) where T : class, IDataEntity
            => _dataStore.GetCollection<T>().DeleteOneAsync(entity.ID);

        public void Dispose()
        {
            // Do nothing
        }
    }
}