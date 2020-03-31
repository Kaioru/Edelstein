using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;

namespace Edelstein.Database.Utils
{
    public class EnumerableDataQuery<T> : IDataQuery<T>
        where T : class, IDataEntity
    {
        private readonly IEnumerable<T> _enumerable;

        public EnumerableDataQuery(IEnumerable<T> enumerable)
            => _enumerable = enumerable;

        public IDataQuery<T> OrderBy<K>(Expression<Func<T, K>> keySelector)
            => new EnumerableDataQuery<T>(_enumerable.OrderBy(keySelector.Compile()));

        public IDataQuery<T> OrderByDescending<K>(Expression<Func<T, K>> keySelector)
            => new EnumerableDataQuery<T>(_enumerable.OrderByDescending(keySelector.Compile()));

        public IDataQuery<T> Where(Expression<Func<T, bool>> predicate)
            => new EnumerableDataQuery<T>(_enumerable.Where(predicate.Compile()));

        public IDataQueryResult<T> Limit(int limit)
            => new EnumerableDataQuery<T>(_enumerable.Take(limit));

        public IDataQueryResult<T> Skip(int offset)
            => new EnumerableDataQuery<T>(_enumerable.Skip(offset));

        public IEnumerable<T> ToEnumerable()
            => _enumerable.AsEnumerable();

        public IList<T> ToList()
            => _enumerable.ToList();

        public IImmutableList<T> ToImmutableList()
            => _enumerable.ToImmutableList();

        public T[] ToArray()
            => _enumerable.ToArray();

        public T First()
            => _enumerable.First();

        public T FirstOrDefault()
            => _enumerable.FirstOrDefault();

        public T Single()
            => _enumerable.Single();

        public T SingleOrDefault()
            => _enumerable.SingleOrDefault();

        public int Count()
            => _enumerable.Count();
    }
}