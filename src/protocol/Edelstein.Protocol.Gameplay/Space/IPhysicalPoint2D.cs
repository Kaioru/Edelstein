using Edelstein.Protocol.Util.Repositories;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Space
{
    public interface IPhysicalPoint2D : IRepositoryEntry<int>
    {
        Point2D Position { get; }
    }
}
