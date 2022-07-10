using System;
using System.Threading.Tasks;

namespace Edelstein.Protocol.Datastore
{
    public interface IDataSession : IDisposable
    {
        IDataSessionBatch NewBatch();

        IDataQuery<T> Query<T>() where T : class, IDataDocument;

        Task<bool> Exists<T>(int id) where T : class, IDataDocument;
        Task<T> Retrieve<T>(int id) where T : class, IDataDocument;
        Task Insert<T>(T entity) where T : class, IDataDocument;
        Task Update<T>(T entity) where T : class, IDataDocument;
        Task Delete<T>(T entity) where T : class, IDataDocument;
        Task Delete<T>(int id) where T : class, IDataDocument;
    }
}
