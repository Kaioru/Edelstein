using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Edelstein.Database.Utils
{
    public class QueryableDataQuery<T> : IDataQuery<T>
        where T : class, IDataEntity
    {
        private readonly IQueryable<T> _queryable;

        public QueryableDataQuery(IQueryable<T> queryable)
            => _queryable = queryable;

        public IEnumerator<T> GetEnumerator()
            => _queryable.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _queryable.GetEnumerator();
    }
}