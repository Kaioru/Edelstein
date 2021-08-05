using Edelstein.Protocol.Datastore;
using LiteDB;

namespace Edelstein.Common.Datastore.LiteDB
{
    public class LiteDataStore : IDataStore
    {
        private readonly ILiteRepository _repository;

        public LiteDataStore(ILiteRepository repository)
            => _repository = repository;

        public IDataSessionBatch StartBatch()
            => StartSession().NewBatch();

        public IDataSession StartSession()
            => new LiteDataSession(_repository);
    }
}
