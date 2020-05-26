using System.Threading.Tasks;
using Marten;

namespace Edelstein.Core.Database.Postgres
{
    public class MartenDataStore : IDataStore
    {
        private readonly IDocumentStore _store;

        public MartenDataStore(IDocumentStore store)
            => _store = store;

        public Task Initialize()
            => Task.CompletedTask;

        public IDataSession StartSession()
            => new MartenDataSession(_store.OpenSession());

        public IDataBatch StartBatch()
            => StartSession().Batch();
    }
}