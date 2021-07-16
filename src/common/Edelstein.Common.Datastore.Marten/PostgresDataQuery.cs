using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Edelstein.Protocol.Datastore;

namespace Edelstein.Common.Datastore.Marten
{
    public class PostgresDataQuery<T> : IDataQuery<T> where T : class, IDataDocument
    {
        private readonly IEnumerable<T> _enumerable;

        public PostgresDataQuery(IEnumerable<T> enumerable)
            => _enumerable = enumerable;

        public IDataQuery<T> Where(Expression<Func<T, bool>> predicate)
            => new PostgresDataQuery<T>(_enumerable.Where(predicate.Compile()));

        public IDataQuery<T> OrderBy<K>(Expression<Func<T, K>> keySelector)
            => new PostgresDataQuery<T>(_enumerable.OrderBy(keySelector.Compile()));
        public IDataQuery<T> OrderByDescending<K>(Expression<Func<T, K>> keySelector)
            => new PostgresDataQuery<T>(_enumerable.OrderByDescending(keySelector.Compile()));

        public IDataQuery<T> Limit(int limit)
            => new PostgresDataQuery<T>(_enumerable.Take(limit));
        public IDataQuery<T> Skip(int offset)
            => new PostgresDataQuery<T>(_enumerable.Skip(offset));

        public T First()
            => _enumerable.First();
        public T FirstOrDefault()
            => _enumerable.FirstOrDefault();

        public IEnumerable<T> All()
            => _enumerable.ToList();

        public int Count()
            => _enumerable.Count();
    }
}
