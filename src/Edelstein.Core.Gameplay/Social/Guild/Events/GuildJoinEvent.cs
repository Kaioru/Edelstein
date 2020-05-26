using System.Collections.Generic;
using Edelstein.Core.Entities.Social;

namespace Edelstein.Core.Gameplay.Social.Guild.Events
{
    public class GuildJoinEvent : IGuildMemberEvent
    {
        public int GuildID { get; }
        public int GuildMemberID { get; }

        public Entities.Social.Guild Guild { get; }
        public ICollection<GuildMember> GuildMembers { get; }

        public GuildJoinEvent(int guildID, int guildMemberID, Entities.Social.Guild guild,
            ICollection<GuildMember> guildMembers)
        {
            GuildID = guildID;
            GuildMemberID = guildMemberID;
            Guild = guild;
            GuildMembers = guildMembers;
        }
    }
}