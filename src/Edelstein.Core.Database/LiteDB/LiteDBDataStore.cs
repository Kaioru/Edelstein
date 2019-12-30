using System.Threading.Tasks;
using LiteDB;

namespace Edelstein.Database.LiteDB
{
    public class LiteDBDataStore : IDataStore
    {
        private readonly LiteRepository _repository;

        public LiteDBDataStore(LiteRepository repository)
            => _repository = repository;

        public Task Initialize()
            => Task.CompletedTask;

        public IDataSession StartSession()
            => new LiteDBDataSession(_repository);

        public IDataBatch StartBatch()
            => StartSession().Batch();
    }
}