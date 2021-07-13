using Edelstein.Protocol.Util.Repositories;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Space
{
    public interface IPhysicalLine2D : IRepositoryEntry<int>
    {
        Line2D Line { get; }
    }
}
