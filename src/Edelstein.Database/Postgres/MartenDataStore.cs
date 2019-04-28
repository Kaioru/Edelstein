using Marten;

namespace Edelstein.Database.Postgres
{
    public class MartenDataStore : IDataStore
    {
        private readonly IDocumentStore _store;

        public MartenDataStore(IDocumentStore store)
            => _store = store;

        public IDataSession OpenSession()
            => new MartenDataSession(_store.OpenSession());

        public void Dispose()
            => _store?.Dispose();
    }
}