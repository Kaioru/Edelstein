using System.Collections.Generic;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Social
{
    public interface IGuild : IRepositoryEntry<int>
    {
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
