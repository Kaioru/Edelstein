using System;
using System.Linq.Expressions;

namespace Edelstein.Core.Database
{
    public interface IDataQuery<T> : IDataQueryResult<T>
        where T : class, IDataEntity
    {
        IDataQuery<T> OrderBy<K>(Expression<Func<T, K>> keySelector);
        IDataQuery<T> OrderByDescending<K>(Expression<Func<T, K>> keySelector);

        IDataQuery<T> Where(Expression<Func<T, bool>> predicate);
    }
}