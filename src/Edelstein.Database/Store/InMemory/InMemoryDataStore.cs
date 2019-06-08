using System;
using System.Collections;
using System.Collections.Generic;

namespace Edelstein.Database.Store.InMemory
{
    public class InMemoryDataStore : IDataStore
    {
        public IDictionary<Type, IList> Database { get; }
        public IDictionary<Type, int> Keys { get; }

        public InMemoryDataStore()
        {
            Database = new Dictionary<Type, IList>();
            Keys = new Dictionary<Type, int>();
        }

        public IDataSession OpenSession()
            => new InMemoryDataSession(Database, Keys);

        public void Dispose()
            => Database.Clear();
    }
}