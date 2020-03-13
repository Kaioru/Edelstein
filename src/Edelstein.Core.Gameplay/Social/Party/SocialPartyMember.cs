using System.Threading.Tasks;
using Edelstein.Entities.Social;

namespace Edelstein.Core.Gameplay.Social.Party
{
    public class SocialPartyMember : ISocialPartyMember
    {
        private readonly ISocialParty _party;
        private readonly PartyMember _member;

        public int CharacterID => _member.CharacterID;
        public string CharacterName => _member.CharacterName;
        public int Job => _member.Job;
        public int Level => _member.Level;
        public int ChannelID => _member.ChannelID;
        public int FieldID => _member.FieldID;

        public SocialPartyMember(ISocialParty party, PartyMember member)
        {
            _party = party;
            _member = member;
        }

        public Task OnUpdateUserMigration(int channelID, int fieldID)
        {
            _member.ChannelID = channelID;
            _member.FieldID = fieldID;
            return Task.CompletedTask;
        }

        public Task OnUpdateChangeLevelOrJob(int level, int job)
        {
            _member.Level = level;
            _member.Job = job;
            return Task.CompletedTask;
        }
    }
}