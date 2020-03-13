using System.Collections.Generic;
using Edelstein.Entities.Social;

namespace Edelstein.Core.Gameplay.Social.Guild.Events
{
    public class GuildCreateEvent : IGuildEvent
    {
        public int GuildID { get; }

        public Entities.Social.Guild Guild { get; }
        public ICollection<GuildMember> Members { get; }

        public GuildCreateEvent(int guildID, Entities.Social.Guild guild, ICollection<GuildMember> members)
        {
            GuildID = guildID;
            Guild = guild;
            Members = members;
        }
    }
}