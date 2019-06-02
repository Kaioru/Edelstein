using System;
using System.Threading.Tasks;

namespace Edelstein.Database
{
    public interface IDataSession : IDisposable
    {
        IDataQuery<T> Query<T>() where T : class, IDataEntity;

        void Insert<T>(T entity) where T : class, IDataEntity;
        void Update<T>(T entity) where T : class, IDataEntity;
        void Delete<T>(T entity) where T : class, IDataEntity;

        Task InsertAsync<T>(T entity) where T : class, IDataEntity;
        Task UpdateAsync<T>(T entity) where T : class, IDataEntity;
        Task DeleteAsync<T>(T entity) where T : class, IDataEntity;
    }
}