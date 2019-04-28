using System;
using System.Threading.Tasks;

namespace Edelstein.Database
{
    public interface IDataSession : IDisposable
    {
        IDataQuery<T> Query<T>();
        
        void Insert<T>(T entity);
        void Update<T>(T entity);
        void Delete<T>(T entity);

        Task InsertAsync<T>(T entity);
        Task UpdateAsync<T>(T entity);
        Task DeleteAsync<T>(T entity);
    }
}