using System.Collections.Generic;
using System.Linq;

namespace Edelstein.Database
{
    public interface IDataQuery<out T> : IEnumerable<T>
        where T : class, IDataEntity
    {
    }
}