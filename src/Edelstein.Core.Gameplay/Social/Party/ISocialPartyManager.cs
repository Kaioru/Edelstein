using System.Threading.Tasks;
using Edelstein.Entities.Characters;

namespace Edelstein.Core.Gameplay.Social.Party
{
    public interface ISocialPartyManager
    {
        Task<ISocialParty?> Load(Character character);
        Task<ISocialParty> Create(Character character);

        Task Join(ISocialParty party, Character character);
        Task Invite(ISocialParty party, Character character);
        Task ChangeBoss(ISocialParty party, ISocialPartyMember member);
        Task Withdraw(ISocialParty party, ISocialPartyMember member);
        Task Kick(ISocialParty party, ISocialPartyMember member);
        
        Task Disband(ISocialParty party);
    }
}