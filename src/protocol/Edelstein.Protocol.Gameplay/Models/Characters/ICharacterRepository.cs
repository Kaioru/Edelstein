using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Models.Characters;

public interface ICharacterRepository : IQueriedRepository<int, ICharacter>
{
    Task<bool> CheckExistsByName(string name);
    
    Task<ICharacter?> RetrieveByName(string name);
    Task<ICharacter?> RetrieveByAccountWorldAndCharacter(int accountWorld, int character);
    Task<IEnumerable<ICharacter>> RetrieveAllByAccountWorld(int accountWorld);
}
