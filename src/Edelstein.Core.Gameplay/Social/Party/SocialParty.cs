using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Entities.Social;

namespace Edelstein.Core.Gameplay.Social.Party
{
    public class SocialParty : ISocialParty
    {
        private readonly Entities.Social.Party _party;

        public int ID => _party.ID;
        public int BossCharacterID => _party.BossCharacterID;
        public ICollection<ISocialPartyMember> Members { get; }

        public SocialParty(Entities.Social.Party party, IEnumerable<PartyMember> members)
        {
            _party = party;

            Members = members
                .Select<PartyMember, ISocialPartyMember>(p => new SocialPartyMember(this, p))
                .ToImmutableList();
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