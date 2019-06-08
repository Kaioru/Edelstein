using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Edelstein.Database.Store.InMemory
{
    public class InMemoryDataQuery<T> : IDataQuery<T>
    {
        private readonly IQueryable<T> _queryable;
        
        public Type ElementType => _queryable.ElementType;
        public Expression Expression => _queryable.Expression;
        public IQueryProvider Provider => _queryable.Provider;

        public InMemoryDataQuery(IQueryable<T> queryable)
            => _queryable = queryable;

        public IEnumerator<T> GetEnumerator()
            => _queryable.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _queryable.GetEnumerator();
    }
}