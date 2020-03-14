using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Entities.Characters;
using Edelstein.Entities.Social;

namespace Edelstein.Core.Gameplay.Social.Party
{
    public class SocialParty : ISocialParty
    {
        private readonly ISocialPartyManager _manager;
        private readonly Entities.Social.Party _party;

        public int ID => _party.ID;
        public int BossCharacterID => _party.BossCharacterID;
        public ICollection<ISocialPartyMember> Members { get; }

        public SocialParty(
            ISocialPartyManager manager,
            Entities.Social.Party party,
            IEnumerable<PartyMember> members
        )
        {
            _manager = manager;
            _party = party;

            Members = members
                .Select<PartyMember, ISocialPartyMember>(p => new SocialPartyMember(_manager, this, p))
                .ToList();
        }

        public Task Join(Character character)
            => _manager.Join(this, character);

        public Task Disband()
            => _manager.Disband(this);

        public Task Withdraw(ISocialPartyMember member)
            => _manager.Withdraw(this, member);

        public Task Kick(ISocialPartyMember member)
            => _manager.Kick(this, member);

        public Task ChangeBoss(ISocialPartyMember member, bool disconnect = false)
            => _manager.ChangeBoss(this, member, disconnect);

        public Task Chat(string name, string text)
            => _manager.Chat(this, name, text);

        public Task UpdateUserMigration(int characterID, int channelID, int fieldID)
            => _manager.UpdateUserMigration(this, characterID, channelID, fieldID);

        public Task UpdateChangeLevelOrJob(int characterID, int level, int job)
            => _manager.UpdateChangeLevelOrJob(this, characterID, level, job);

        public Task OnUpdateJoin(ISocialPartyMember member)
        {
            Members.Add(member);
            return Task.CompletedTask;
        }

        public Task OnUpdateWithdraw(int characterID)
        {
            Members.Remove(Members.FirstOrDefault(m => m.CharacterID == characterID));
            return Task.CompletedTask;
        }

        public Task OnUpdateBoss(int characterID)
        {
            _party.BossCharacterID = characterID;
            return Task.CompletedTask;
        }

        public Task OnUpdateUserMigration(int characterID, int channelID, int fieldID)
            => Members
                .FirstOrDefault(m => m.CharacterID == characterID)
                ?.OnUpdateUserMigration(channelID, fieldID);

        public Task OnUpdateChangeLevelOrJob(int characterID, int level, int job)
            => Members
                .FirstOrDefault(m => m.CharacterID == characterID)
                ?.OnUpdateChangeLevelOrJob(level, job);
    }
}