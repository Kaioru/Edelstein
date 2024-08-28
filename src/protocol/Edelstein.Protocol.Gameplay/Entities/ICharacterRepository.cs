using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Entities;

public interface ICharacterRepository : IQueriedRepository<int, Character>
{
    Task<bool> CheckExistsByName(string name);
    
    Task<Character?> RetrieveByName(string name);
    Task<Character?> RetrieveByAccountWorldDataAndCharacter(int accountWorldData, int character);
    Task<IEnumerable<Character>> RetrieveAllByAccountWorldData(int accountWorldData);
}
