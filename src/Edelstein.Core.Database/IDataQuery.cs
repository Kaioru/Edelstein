using System.Collections.Generic;

namespace Edelstein.Database
{
    public interface IDataQuery<out T> : IEnumerable<T>
        where T : class, IDataEntity
    {
    }
}