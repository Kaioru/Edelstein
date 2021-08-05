using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Edelstein.Protocol.Datastore
{
    public interface IDataQuery<T> where T : IDataDocument
    {
        IDataQuery<T> Where(Expression<Func<T, bool>> predicate);

        IDataQuery<T> OrderBy<K>(Expression<Func<T, K>> keySelector);
        IDataQuery<T> OrderByDescending<K>(Expression<Func<T, K>> keySelector);

        IDataQuery<T> Limit(int limit);
        IDataQuery<T> Skip(int offset);

        Task<T> First();
        Task<T> FirstOrDefault();

        Task<IEnumerable<T>> All();

        Task<int> Count();
    }
}
