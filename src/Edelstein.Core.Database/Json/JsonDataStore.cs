using System.Threading.Tasks;
using JsonFlatFileDataStore;

namespace Edelstein.Database.Json
{
    public class JsonDataStore : IDataStore
    {
        private readonly DataStore _dataStore;

        public JsonDataStore(DataStore dataStore)
            => _dataStore = dataStore;

        public Task Initialize()
            => Task.CompletedTask;

        public IDataSession StartSession()
            => new JsonDataSession(_dataStore);

        public IDataBatch StartBatch()
            => StartSession().Batch();
    }
}