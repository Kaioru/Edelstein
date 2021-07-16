using System.Threading.Tasks;

namespace Edelstein.Protocol.Datastore
{
    public interface IDataStore
    {
        Task Initialize();

        IDataSession StartSession();
        IDataSessionBatch StartBatch();
    }
}
