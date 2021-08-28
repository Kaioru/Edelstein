using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Edelstein.Protocol.Datastore;

namespace Edelstein.Common.Datastore.LiteDB
{
    public class EnumerableDataQuery<T> : IDataQuery<T> where T : class, IDataDocument
    {
        private readonly IEnumerable<T> _enumerable;

        public EnumerableDataQuery(IEnumerable<T> enumerable)
        {
            _enumerable = enumerable;
        }

        public IDataQuery<T> Where(Expression<Func<T, bool>> predicate)
            => new EnumerableDataQuery<T>(_enumerable.Where(predicate.Compile()));

        public IDataQuery<T> OrderBy<K>(Expression<Func<T, K>> keySelector)
            => new EnumerableDataQuery<T>(_enumerable.OrderBy(keySelector.Compile()));
        public IDataQuery<T> OrderByDescending<K>(Expression<Func<T, K>> keySelector)
            => new EnumerableDataQuery<T>(_enumerable.OrderByDescending(keySelector.Compile()));

        public IDataQuery<T> Limit(int limit)
            => new EnumerableDataQuery<T>(_enumerable.Take(limit));
        public IDataQuery<T> Skip(int offset)
            => new EnumerableDataQuery<T>(_enumerable.Skip(offset));

        public Task<T> First()
            => Task.FromResult(_enumerable.First());

        public Task<T> FirstOrDefault()
            => Task.FromResult(_enumerable.FirstOrDefault());

        public Task<IEnumerable<T>> All()
            => Task.FromResult<IEnumerable<T>>(_enumerable.ToList());

        public Task<int> Count()
            => Task.FromResult(_enumerable.Count());
    }
}
