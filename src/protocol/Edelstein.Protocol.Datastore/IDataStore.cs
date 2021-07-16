namespace Edelstein.Protocol.Datastore
{
    public interface IDataStore
    {
        IDataSession StartSession();
        IDataSessionBatch StartBatch();
    }
}
