using System.Collections.Generic;

namespace Edelstein.Core.Gameplay.Social.Guild
{
    public interface ISocialGuild
    {
        public int ID { get; }

        public string Name { get; }

        public string[] GradeName { get; }

        public int MaxMemberNum { get; }
        public ICollection<ISocialGuildMember> Members { get; }

        public short MarkBg { get; }
        public byte MarkBgColor { get; }
        public short Mark { get; }
        public byte MarkColor { get; }

        public string Notice { get; }
        public int Point { get; }
        public byte Level { get; }
    }
}