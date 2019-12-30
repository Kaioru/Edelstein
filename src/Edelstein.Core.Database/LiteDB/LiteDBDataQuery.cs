using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq.Expressions;
using LiteDB;

namespace Edelstein.Database.LiteDB
{
    public class LiteDBDataQuery<T> : IDataQuery<T>
        where T : class, IDataEntity
    {
        private readonly ILiteQueryable<T> _queryable;

        public LiteDBDataQuery(ILiteQueryable<T> queryable)
            => _queryable = queryable;

        public IDataQuery<T> OrderBy<K>(Expression<Func<T, K>> keySelector)
            => new LiteDBDataQuery<T>(_queryable.OrderBy(keySelector));

        public IDataQuery<T> OrderByDescending<K>(Expression<Func<T, K>> keySelector)
            => new LiteDBDataQuery<T>(_queryable.OrderByDescending(keySelector));

        public IDataQuery<T> Where(Expression<Func<T, bool>> predicate)
            => new LiteDBDataQuery<T>(_queryable.Where(predicate));

        public IDataQueryResult<T> Limit(int limit)
            => new LiteDBDataQueryResult<T>(_queryable.Limit(limit));

        public IDataQueryResult<T> Skip(int offset)
            => new LiteDBDataQueryResult<T>(_queryable.Skip(offset));

        public IEnumerable<T> ToEnumerable()
            => _queryable.ToEnumerable();

        public IList<T> ToList()
            => _queryable.ToList();

        public IImmutableList<T> ToImmutableList()
            => _queryable.ToEnumerable().ToImmutableList();

        public T[] ToArray()
            => _queryable.ToArray();

        public T First()
            => _queryable.First();

        public T FirstOrDefault()
            => _queryable.FirstOrDefault();

        public T Single()
            => _queryable.Single();

        public T SingleOrDefault()
            => _queryable.SingleOrDefault();

        public int Count()
            => _queryable.Count();
    }
}