using Edelstein.Protocol.Gameplay.Characters;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Common.Gameplay.Characters;

public interface ICharacterRepository : IRepository<int, ICharacter>
{
    Task<bool> CheckExistsByName(string name);
    Task<ICharacter?> RetrieveByName(string name);
    Task<IEnumerable<ICharacter>> RetrieveAllByAccountWorld(int accountWorld);
}
