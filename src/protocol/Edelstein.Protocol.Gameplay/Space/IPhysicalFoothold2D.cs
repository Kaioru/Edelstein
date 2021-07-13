using Edelstein.Protocol.Util.Repositories;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Space
{
    public interface IPhysicalFoothold2D : IRepositoryEntry<int>
    {
        Line2D Line { get; }
    }
}
