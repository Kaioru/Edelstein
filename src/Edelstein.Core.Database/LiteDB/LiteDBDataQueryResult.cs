using System.Collections.Generic;
using System.Collections.Immutable;
using LiteDB;

namespace Edelstein.Core.Database.LiteDB
{
    public class LiteDBDataQueryResult<T> : IDataQueryResult<T>
        where T : class, IDataEntity
    {
        private readonly ILiteQueryableResult<T> _queryable;

        public LiteDBDataQueryResult(ILiteQueryableResult<T> queryable)
            => _queryable = queryable;

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