using System.Collections.Generic;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Social
{
    public interface IParty : IRepositoryEntry<int>
    {
        int Boss { get; }
        ICollection<IPartyMember> Members { get; }
    }
}
