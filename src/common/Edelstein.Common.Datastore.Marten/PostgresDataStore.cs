using Edelstein.Protocol.Datastore;
using Marten;

namespace Edelstein.Common.Datastore.Marten
{
    public class PostgresDataStore : IDataStore
    {
        private readonly IDocumentStore _store;

        public PostgresDataStore(IDocumentStore store)
            => _store = store;

        public IDataSessionBatch StartBatch()
            => new PostgresDataSessionBatch(_store.OpenSession());

        public IDataSession StartSession()
            => new PostgresDataSession(_store.OpenSession());
    }
}
