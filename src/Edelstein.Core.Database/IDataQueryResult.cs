using System.Collections.Generic;
using System.Collections.Immutable;

namespace Edelstein.Database
{
    public interface IDataQueryResult<T>
        where T : class, IDataEntity
    {
        IDataQueryResult<T> Limit(int limit);
        IDataQueryResult<T> Skip(int offset);

        IEnumerable<T> ToEnumerable();
        IList<T> ToList();
        IImmutableList<T> ToImmutableList();
        T[] ToArray();

        T First();
        T FirstOrDefault();
        T Single();
        T SingleOrDefault();

        int Count();
    }
}