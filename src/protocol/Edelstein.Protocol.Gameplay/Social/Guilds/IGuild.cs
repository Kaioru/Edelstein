using System.Collections.Generic;

namespace Edelstein.Protocol.Gameplay.Social.Guilds
{
    public interface IGuild
    {
        int ID { get; }

        string Name { get; }
        string[] Grade { get; }

        int MaxMemberNum { get; }
        ICollection<IGuildMember> Members { get; }

        short MarkBg { get; }
        byte MarkBgColor { get; }
        short Mark { get; }
        byte MarkColor { get; }

        string Notice { get; }
        int Point { get; }
        byte Level { get; }
    }
}
