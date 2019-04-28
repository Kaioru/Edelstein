using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Marten.Linq;

namespace Edelstein.Database.Postgres
{
    public class MartenDataQuery<T> : IDataQuery<T>
    {
        private readonly IMartenQueryable<T> _queryable;

        public Type ElementType => _queryable.ElementType;
        public Expression Expression => _queryable.Expression;
        public IQueryProvider Provider => _queryable.Provider;

        public MartenDataQuery(IMartenQueryable<T> queryable)
            => _queryable = queryable;

        public IEnumerator<T> GetEnumerator()
            => _queryable.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _queryable.GetEnumerator();
    }
}