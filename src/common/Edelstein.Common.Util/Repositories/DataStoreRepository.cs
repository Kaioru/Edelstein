using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Datastore;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Common.Util.Repositories
{
    public class DataStoreRepository<TEntry> : IRepository<int, TEntry> where TEntry : class, IDataDocument
    {
        protected IDataStore Store { get; }

        public DataStoreRepository(IDataStore store)
            => Store = store;

        public Task<TEntry> Retrieve(int key)
        {
            using var session = Store.StartSession();
            return session.Retrieve<TEntry>(key);
        }

        public async Task<IEnumerable<TEntry>> Retrieve(IEnumerable<int> keys)
        {
            using var session = Store.StartSession();
            var result = await Task.WhenAll(keys.Select(key => session.Retrieve<TEntry>(key)));
            return result;
        }

        public Task Insert(TEntry entry)
        {
            using var session = Store.StartSession();
            return session.Insert(entry);
        }

        public async Task Insert(IEnumerable<TEntry> entries)
        {
            using var batch = Store.StartBatch();

            await Task.WhenAll(entries.Select(entry => batch.Insert(entry)));
            await batch.Save();
        }

        public Task Update(TEntry entry)
        {
            using var session = Store.StartSession();
            return session.Update(entry);
        }

        public async Task Update(IEnumerable<TEntry> entries)
        {
            using var batch = Store.StartBatch();

            await Task.WhenAll(entries.Select(entry => batch.Update(entry)));
            await batch.Save();
        }

        public Task Delete(int key)
        {
            using var session = Store.StartSession();
            return session.Delete<TEntry>(key);
        }

        public Task Delete(TEntry entry)
        {
            using var session = Store.StartSession();
            return session.Delete(entry);
        }

        public async Task Delete(IEnumerable<int> keys)
        {
            using var batch = Store.StartBatch();

            await Task.WhenAll(keys.Select(key => batch.Delete<TEntry>(key)));
            await batch.Save();
        }

        public async Task Delete(IEnumerable<TEntry> entries)
        {
            using var batch = Store.StartBatch();

            await Task.WhenAll(entries.Select(entry => batch.Delete(entry)));
            await batch.Save();
        }
    }
}
