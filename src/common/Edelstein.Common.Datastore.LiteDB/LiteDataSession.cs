using System.Threading.Tasks;
using Edelstein.Protocol.Datastore;
using LiteDB;

namespace Edelstein.Common.Datastore.LiteDB
{
    public class LiteDataSession : IDataSession
    {
        private readonly ILiteRepository _repository;

        public LiteDataSession(ILiteRepository repository)
            => _repository = repository;

        public IDataSessionBatch NewBatch()
            => new LiteDataSessionBatch(this);

        public IDataQuery<T> Query<T>() where T : class, IDataDocument
            => new LiteDataQuery<T>(_repository.Query<T>());

        public async Task<bool> Exists<T>(int id) where T : class, IDataDocument
             => (await Query<T>().Where(o => o.ID == id).Count()) > 0;
        
        public Task<T> Retrieve<T>(int id) where T : class, IDataDocument
            => Task.FromResult(_repository.SingleById<T>(id));

        public Task Insert<T>(T entity) where T : class, IDataDocument
            => Task.Run(() => _repository.Insert(entity));

        public Task Update<T>(T entity) where T : class, IDataDocument
            => Task.Run(() => _repository.Update(entity));

        public Task Delete<T>(T entity) where T : class, IDataDocument
            => Task.Run(() => _repository.Delete<T>(entity.ID));

        public Task Delete<T>(int id) where T : class, IDataDocument
            => Task.Run(() => _repository.Delete<T>(id));

        public void Dispose() { }
    }
}
