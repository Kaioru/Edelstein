using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Edelstein.Protocol.Datastore;
using LiteDB;

namespace Edelstein.Common.Datastore.LiteDB
{
    public class LiteDataQuery<T> : IDataQuery<T> where T : class, IDataDocument
    {
        private readonly ILiteQueryable<T> _queryable;

        public LiteDataQuery(ILiteQueryable<T> queryable)
            => _queryable = queryable;

        public IDataQuery<T> Where(Expression<Func<T, bool>> predicate)
            => new LiteDataQuery<T>(_queryable.Where(predicate));

        public IDataQuery<T> OrderBy<K>(Expression<Func<T, K>> keySelector)
            => new LiteDataQuery<T>(_queryable.OrderBy(keySelector));
        public IDataQuery<T> OrderByDescending<K>(Expression<Func<T, K>> keySelector)
            => new LiteDataQuery<T>(_queryable.OrderByDescending(keySelector));

        public IDataQuery<T> Limit(int limit)
            => new LiteDataQueryResult<T>(_queryable, _queryable.Limit(limit));
        public IDataQuery<T> Skip(int offset)
            => new LiteDataQueryResult<T>(_queryable, _queryable.Skip(offset));

        public T First()
            => _queryable.First();
        public T FirstOrDefault()
            => _queryable.FirstOrDefault();

        public IEnumerable<T> All()
            => _queryable.ToList();

        public int Count()
            => _queryable.Count();
    }
}
