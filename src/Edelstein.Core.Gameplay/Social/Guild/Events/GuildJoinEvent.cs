using Edelstein.Entities.Social;

namespace Edelstein.Core.Gameplay.Social.Guild.Events
{
    public class GuildJoinEvent : IGuildMemberEvent
    {
        public int GuildID { get; }
        public int GuildMemberID { get; }

        public GuildMember Member { get; }

        public GuildJoinEvent(int guildID, int guildMemberID, GuildMember member)
        {
            GuildID = guildID;
            GuildMemberID = guildMemberID;
            Member = member;
        }
    }
}