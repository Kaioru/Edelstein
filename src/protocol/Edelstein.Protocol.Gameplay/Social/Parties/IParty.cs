using System.Collections.Generic;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Social.Parties
{
    public interface IParty : IRepositoryEntry<int>
    {
        int BossCharacterID { get; }

        ICollection<IPartyMember> Members { get; }
    }
}
