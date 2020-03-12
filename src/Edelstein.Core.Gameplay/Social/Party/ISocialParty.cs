using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Entities.Characters;

namespace Edelstein.Core.Gameplay.Social.Party
{
    public interface ISocialParty
    {
        int ID { get; }

        int BossCharacterID { get; }
        ICollection<ISocialPartyMember> Members { get; }

        Task Join(Character character);
        Task Invite(Character character);
        Task Disband();
    }
}