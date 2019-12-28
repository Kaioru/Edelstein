using System.Collections;
using System.Collections.Generic;

namespace Edelstein.Database.Utils
{
    public class EnumerableDataQuery<T> : IDataQuery<T>
        where T : class, IDataEntity
    {
        private readonly IEnumerable<T> _enumerable;

        public EnumerableDataQuery(IEnumerable<T> enumerable)
            => _enumerable = enumerable;

        public IEnumerator<T> GetEnumerator()
            => _enumerable.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _enumerable.GetEnumerator();
    }
}