using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiteDB;

namespace Edelstein.Database.Store.LiteDB
{
    public class LiteDBDataBatch : IDataBatch
    {
        private readonly LiteDatabase _database;
        private readonly Queue<Action<LiteDatabase>> _actions;

        public LiteDBDataBatch(LiteDatabase database)
        {
            _database = database;
            _actions = new Queue<Action<LiteDatabase>>();
        }

        public void Insert<T>(T entity) where T : class, IDataEntity
            => _actions.Enqueue(d => d.GetCollection<T>().Insert(entity));

        public void Update<T>(T entity) where T : class, IDataEntity
            => _actions.Enqueue(d => d.GetCollection<T>().Update(entity));

        public void Delete<T>(T entity) where T : class, IDataEntity
            => _actions.Enqueue(d => d.GetCollection<T>().Delete(entity.ID));

        public void SaveChanges()
        {
            foreach (var action in _actions)
                action.Invoke(_database);
        }

        public Task SaveChangesAsync()
            => Task.Run(SaveChanges);

        public void Dispose()
        {
            // Do nothing
        }
    }
}