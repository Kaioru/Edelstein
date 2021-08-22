using System;
using System.Collections.Generic;
using Edelstein.Protocol.Datastore;

namespace Edelstein.Protocol.Gameplay.Social.Guilds
{
    public record GuildRecord : IGuild, IDataDocument
    {
        public int ID { get; init; }

        public string Name { get; }
        public string[] Grade { get; }

        public int MaxMemberNum { get; }
        public ICollection<IGuildMember> Members { get; }

        public short MarkBg { get; }
        public byte MarkBgColor { get; }
        public short Mark { get; }
        public byte MarkColor { get; }

        public string Notice { get; }
        public int Point { get; }
        public byte Level { get; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public GuildRecord()
        {
            Members = new List<IGuildMember>();
        }
    }
}
