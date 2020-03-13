using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Core.Gameplay.Social.Party
{
    public interface ISocialParty
    {
        int ID { get; }

        int BossCharacterID { get; }
        ICollection<ISocialPartyMember> Members { get; }

        Task OnUpdateUserMigration(int characterID, int channelID, int fieldID);
        Task OnUpdateChangeLevelOrJob(int characterID, int level, int job);
    }
}