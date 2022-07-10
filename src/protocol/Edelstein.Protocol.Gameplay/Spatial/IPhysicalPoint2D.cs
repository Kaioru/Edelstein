using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Spatial
{
    public interface IPhysicalPoint2D : IPhysicalObject2D
    {
        Point2D Position { get; }
    }
}
