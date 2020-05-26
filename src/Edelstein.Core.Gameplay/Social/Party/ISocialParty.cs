using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Core.Entities.Characters;

namespace Edelstein.Core.Gameplay.Social.Party
{
    public interface ISocialParty
    {
        int ID { get; }

        int BossCharacterID { get; }
        ICollection<ISocialPartyMember> Members { get; }

        Task Join(Character character);
        Task Disband();
        Task Withdraw(ISocialPartyMember member);
        Task Kick(ISocialPartyMember member);
        Task ChangeBoss(ISocialPartyMember member, bool disconnect = false);

        Task Chat(string name, string text);

        Task UpdateUserMigration(int characterID, int channelID, int fieldID);
        Task UpdateChangeLevelOrJob(int characterID, int level, int job);

        Task OnUpdateJoin(ISocialPartyMember member);
        Task OnUpdateWithdraw(int characterID);
        Task OnUpdateBoss(int characterID);
        Task OnUpdateUserMigration(int characterID, int channelID, int fieldID);
        Task OnUpdateChangeLevelOrJob(int characterID, int level, int job);
    }
}