namespace Edelstein.Database.Store
{
    public interface IDataAction
    {
        void Insert<T>(T entity) where T : class, IDataEntity;
        void Update<T>(T entity) where T : class, IDataEntity;
        void Delete<T>(T entity) where T : class, IDataEntity;
    }
}