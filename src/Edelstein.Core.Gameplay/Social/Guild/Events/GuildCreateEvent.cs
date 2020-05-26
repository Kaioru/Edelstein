using System.Collections.Generic;
using Edelstein.Core.Entities.Social;

namespace Edelstein.Core.Gameplay.Social.Guild.Events
{
    public class GuildCreateEvent : IGuildEvent
    {
        public int GuildID { get; }

        public Entities.Social.Guild Guild { get; }
        public ICollection<GuildMember> GuildMembers { get; }

        public GuildCreateEvent(int guildID, Entities.Social.Guild guild, ICollection<GuildMember> guildMembers)
        {
            GuildID = guildID;
            Guild = guild;
            GuildMembers = guildMembers;
        }
    }
}