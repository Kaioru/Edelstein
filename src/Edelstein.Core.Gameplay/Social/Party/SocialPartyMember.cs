using System.Threading.Tasks;
using Edelstein.Entities.Social;

namespace Edelstein.Core.Gameplay.Social.Party
{
    public class SocialPartyMember : ISocialPartyMember
    {
        private readonly ISocialPartyManager _manager;
        private readonly ISocialParty _party;
        private readonly PartyMember _member;

        public int CharacterID => _member.CharacterID;
        public string CharacterName => _member.CharacterName;
        public int Job => _member.Job;
        public int Level => _member.Level;
        public int ChannelID => _member.ChannelID;

        public SocialPartyMember(
            ISocialPartyManager manager,
            ISocialParty party,
            PartyMember member
        )
        {
            _manager = manager;
            _party = party;
            _member = member;
        }

        public Task ChangeBoss()
            => _manager.ChangeBoss(_party, this);

        public Task Withdraw()
            => _manager.Withdraw(_party, this);

        public Task Kick()
            => _manager.Kick(_party, this);
    }
}