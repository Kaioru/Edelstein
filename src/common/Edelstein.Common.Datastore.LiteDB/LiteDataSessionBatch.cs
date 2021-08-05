using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Protocol.Datastore;

namespace Edelstein.Common.Datastore.LiteDB
{
    public class LiteDataSessionBatch : IDataSessionBatch
    {
        private readonly IDataSession _session;
        private readonly Queue<Action<IDataSession>> _queue;

        public LiteDataSessionBatch(IDataSession session)
        {
            _session = session;
            _queue = new Queue<Action<IDataSession>>();
        }

        public IDataSessionBatch NewBatch()
            => new LiteDataSessionBatch(_session);

        public IDataQuery<T> Query<T>() where T : class, IDataDocument
            => _session.Query<T>();

        public Task<T> Retrieve<T>(int id) where T : class, IDataDocument
            => _session.Retrieve<T>(id);

        public Task Insert<T>(T entity) where T : class, IDataDocument
        {
            _queue.Enqueue(s => s.Insert(entity));
            return Task.CompletedTask;
        }

        public Task Update<T>(T entity) where T : class, IDataDocument
        {
            _queue.Enqueue(s => s.Update(entity));
            return Task.CompletedTask;
        }

        public Task Delete<T>(T entity) where T : class, IDataDocument
        {
            _queue.Enqueue(s => s.Delete(entity));
            return Task.CompletedTask;
        }

        public Task Delete<T>(int id) where T : class, IDataDocument
        {
            _queue.Enqueue(s => s.Delete<T>(id));
            return Task.CompletedTask;
        }

        public Task Discard()
        {
            _queue.Clear();
            return Task.CompletedTask;
        }

        public Task Save()
        {
            foreach (var action in _queue)
                action.Invoke(_session);
            return Task.CompletedTask;
        }

        public void Dispose()
            => Discard();
    }
}
