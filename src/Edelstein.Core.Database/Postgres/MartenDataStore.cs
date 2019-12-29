using System.Threading.Tasks;
using Marten;

namespace Edelstein.Database.Postgres
{
    public class MartenDataStore : IDataStore
    {
        private readonly IDocumentStore _store;

        public MartenDataStore(IDocumentStore store)
            => _store = store;

        public Task Initialize()
            => Task.CompletedTask;

        public IDataSession StartSession()
        {
            throw new System.NotImplementedException();
        }

        public IDataBatch StartBatch()
        {
            throw new System.NotImplementedException();
        }
    }
}