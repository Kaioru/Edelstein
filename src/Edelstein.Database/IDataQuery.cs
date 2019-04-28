using System.Linq;

namespace Edelstein.Database
{
    public interface IDataQuery<out T> : IQueryable<T>
    {
    }
}