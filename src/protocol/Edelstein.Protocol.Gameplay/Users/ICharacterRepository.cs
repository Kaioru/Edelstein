using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Users
{
    public interface ICharacterRepository : IRepository<int, Character>
    {
        Task<bool> CheckExistsByName(string name);

        Task<Character> RetrieveByName(string name);
        Task<IEnumerable<Character>> RetrieveAllByAccountWorld(int accountworld);
    }
}
