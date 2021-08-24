using System;
using System.Collections.Generic;
using Edelstein.Protocol.Datastore;

namespace Edelstein.Protocol.Gameplay.Social.Guilds
{
    public record GuildRecord : IGuild, IDataDocument
    {
        public int ID { get; init; }

        public string Name { get; set; }
        public string[] Grade { get; set; }

        public int MaxMemberNum { get; set; }
        public ICollection<IGuildMember> Members { get; }

        public short MarkBg { get; set; }
        public byte MarkBgColor { get; set; }
        public short Mark { get; set; }
        public byte MarkColor { get; set; }

        public string Notice { get; set; }
        public int Point { get; set; }
        public byte Level { get; set; }

        public DateTime DateDocumentCreated { get; set; }
        public DateTime DateDocumentUpdated { get; set; }

        public GuildRecord()
        {
            Members = new List<IGuildMember>();
        }
    }
}
