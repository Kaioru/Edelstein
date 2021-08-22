using System;
using System.Collections.Generic;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Social
{
    public interface IGuildInfo
    {
        int ID { get; }

        string Name { get; }
        string[] Grade { get; }

        int MaxMemberNum { get; }
        ICollection<IGuildMemberInfo> Members { get; }

        short MarkBg { get; }
        byte MarkBgColor { get; }
        short Mark { get; }
        byte MarkColor { get; }

        string Notice { get; }
        int Point { get; }
        byte Level { get; }
    }
}
