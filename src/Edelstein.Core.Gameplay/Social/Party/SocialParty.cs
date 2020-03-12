using System.Collections.Generic;
using System.Collections.Immutable;
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

        public SocialParty(ISocialPartyManager manager, Entities.Social.Party party)
        {
            _manager = manager;
            _party = party;

            Members = _party.Members
                .Select<PartyMember, ISocialPartyMember>(m => new SocialPartyMember(manager, this, m))
                .ToImmutableList();
        }

        public Task Join(Character character)
            => _manager.Join(this, character);

        public Task Invite(Character character)
            => _manager.Invite(this, character);

        public Task Disband()
            => _manager.Disband(this);
    }
}