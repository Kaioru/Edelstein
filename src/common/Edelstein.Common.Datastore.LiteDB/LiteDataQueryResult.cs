using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Edelstein.Protocol.Datastore;
using LiteDB;

namespace Edelstein.Common.Datastore.LiteDB
{
    public class LiteDataQueryResult<T> : IDataQuery<T> where T : class, IDataDocument
    {
        private readonly ILiteQueryable<T> _queryable;
        private readonly ILiteQueryableResult<T> _result;

        public LiteDataQueryResult(ILiteQueryable<T> queryable, ILiteQueryableResult<T> result)
        {
            _queryable = queryable;
            _result = result;
        }

        public IDataQuery<T> Where(Expression<Func<T, bool>> predicate)
            => new LiteDataQuery<T>(_queryable.Where(predicate));

        public IDataQuery<T> OrderBy<K>(Expression<Func<T, K>> keySelector)
            => new LiteDataQuery<T>(_queryable.OrderBy(keySelector));
        public IDataQuery<T> OrderByDescending<K>(Expression<Func<T, K>> keySelector)
            => new LiteDataQuery<T>(_queryable.OrderByDescending(keySelector));

        public IDataQuery<T> Limit(int limit)
            => new LiteDataQueryResult<T>(_queryable, _result.Limit(limit));
        public IDataQuery<T> Skip(int offset)
            => new LiteDataQueryResult<T>(_queryable, _result.Skip(offset));

        public Task<T> First()
            => Task.FromResult(_result.First());

        public Task<T> FirstOrDefault()
            => Task.FromResult(_result.FirstOrDefault());

        public Task<IEnumerable<T>> All()
            => Task.FromResult<IEnumerable<T>>(_result.ToList());

        public Task<int> Count()
            => Task.FromResult(_result.Count());
    }
}
