
using LiteDB;

namespace Edelstein.Database.LiteDB
{
    public class LiteDBDataStore : IDataStore
    {
        private readonly string _path;

        public LiteDBDataStore(string path)
        {
            _path = path;
        }

        public IDataSession OpenSession()
            => new LiteDBDataSession(new LiteDatabase(_path, BsonMapper.Global));

        public void Dispose()
        {
            // Do nothing
        }
    }
}