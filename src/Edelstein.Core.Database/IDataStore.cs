using System.Threading.Tasks;


namespace Edelstein.Core.Database
{
    public interface IDataStore
    {
        Task Initialize();

        IDataSession StartSession();
        IDataBatch StartBatch();
    }
}