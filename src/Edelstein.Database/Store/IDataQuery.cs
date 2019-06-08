using System.Linq;

namespace Edelstein.Database.Store
{
    public interface IDataQuery<out T> : IQueryable<T>
    {
    }
}