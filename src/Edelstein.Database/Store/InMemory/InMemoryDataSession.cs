using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edelstein.Database.Store.InMemory
{
    public class InMemoryDataSession : IDataSession
    {
        private readonly IDictionary<Type, IList> _database;
        private readonly IDictionary<Type, int> _keys;

        public InMemoryDataSession(IDictionary<Type, IList> database, IDictionary<Type, int> keys)
        {
            _database = database;
            _keys = keys;
        }

        private IList GetCollection<T>()
        {
            if (!_database.ContainsKey(typeof(T)))
                _database[typeof(T)] = new List<T>();
            return _database[typeof(T)];
        }

        public IDataQuery<T> Query<T>() where T : class, IDataEntity
            => new InMemoryDataQuery<T>(GetCollection<T>().OfType<T>().AsQueryable());

        public void Insert<T>(T entity) where T : class, IDataEntity
        {
            if (!_keys.ContainsKey(typeof(T)))
                _keys[typeof(T)] = 0;

            var key = _keys[typeof(T)];

            _keys[typeof(T)] = ++key;

            entity.GetType().GetProperty("ID")
                ?.SetValue(entity, key, null);
            GetCollection<T>().Add(entity);
        }

        public void Update<T>(T entity) where T : class, IDataEntity
        {
            // Do nothing, because sharing memory
        }

        public void Delete<T>(T entity) where T : class, IDataEntity
            => GetCollection<T>().Remove(entity);

        public Task InsertAsync<T>(T entity) where T : class, IDataEntity
        {
            Insert<T>(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync<T>(T entity) where T : class, IDataEntity
        {
            // Do nothing, because sharing memory
            return Task.CompletedTask;
        }

        public Task DeleteAsync<T>(T entity) where T : class, IDataEntity
        {
            Delete<T>(entity);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            // Do nothing
        }
    }
}