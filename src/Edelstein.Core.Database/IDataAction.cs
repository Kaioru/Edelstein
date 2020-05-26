using System.Threading.Tasks;

namespace Edelstein.Core.Database
{
    public interface IDataAction
    {
        T Retrieve<T>(int id) where T : class, IDataEntity;
        void Insert<T>(T entity) where T : class, IDataEntity;
        void Update<T>(T entity) where T : class, IDataEntity;
        void Delete<T>(T entity) where T : class, IDataEntity;
        void Delete<T>(int id) where T : class, IDataEntity;

        Task<T> RetrieveAsync<T>(int id) where T : class, IDataEntity;
        Task InsertAsync<T>(T entity) where T : class, IDataEntity;
        Task UpdateAsync<T>(T entity) where T : class, IDataEntity;
        Task DeleteAsync<T>(T entity) where T : class, IDataEntity;
        Task DeleteAsync<T>(int id) where T : class, IDataEntity;
    }
}