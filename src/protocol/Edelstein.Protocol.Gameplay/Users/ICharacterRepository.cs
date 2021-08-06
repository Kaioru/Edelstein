using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Users
{
    public interface ICharacterRepository : IRepository<int, Character>
    {
        Task<IEnumerable<Character>> RetrieveAllByAccountWorld(int accountworld);

        Task<bool> CheckExistsByName(string name);
    }
}
