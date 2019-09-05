using System;
using System.Threading.Tasks;

namespace Edelstein.Database.Store
{
    public interface IDataSession : IDataAction, IDisposable
    {
        IDataQuery<T> Query<T>() where T : class, IDataEntity;
        IDataBatch Batch();

        Task InsertAsync<T>(T entity) where T : class, IDataEntity;
        Task UpdateAsync<T>(T entity) where T : class, IDataEntity;
        Task DeleteAsync<T>(T entity) where T : class, IDataEntity;
    }
}