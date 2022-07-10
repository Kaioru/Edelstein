using System.Collections.Generic;
using System.Linq;
using Edelstein.Protocol.Gameplay.Social;
using Edelstein.Protocol.Services.Contracts.Social;

namespace Edelstein.Common.Gameplay.Social
{
    public class Guild : IGuild
    {
        public int ID { get; }
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

        public Guild(GuildContract contract)
        {
            ID = contract.Id;
            Name = contract.Name;
            Grade = contract.Grade.ToArray();
            MaxMemberNum = contract.MaxMemberNum;
            Members = contract.Members.Select<GuildMemberContract, IGuildMember>(m => new GuildMember(m)).ToList();
            MarkBg = (short)contract.MarkBg;
            MarkBgColor = (byte)contract.MarkBgColor;
            Mark = (short)contract.Mark;
            MarkColor = (byte)contract.MarkColor;
            Notice = contract.Notice;
            Point = contract.Point;
            Level = (byte)contract.Level;
        }
    }
}
