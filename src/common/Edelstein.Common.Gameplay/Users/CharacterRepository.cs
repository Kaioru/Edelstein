using Edelstein.Common.Util.Repositories;
using Edelstein.Protocol.Datastore;
using Edelstein.Protocol.Gameplay.Users;

namespace Edelstein.Common.Gameplay.Users
{
    public class CharacterRepository : DataStoreRepository<Character>, ICharacterRepository
    {
        public CharacterRepository(IDataStore store) : base(store) { }
    }
}
