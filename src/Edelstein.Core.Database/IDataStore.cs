using System.Threading.Tasks;

namespace Edelstein.Database
{
    public interface IDataStore
    {
        Task Initialize();

        IDataSession StartSession();
        IDataBatch StartBatch();
    }
}