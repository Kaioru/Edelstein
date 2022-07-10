using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Spatial
{
    public interface IPhysicalLine2D : IPhysicalObject2D
    {
        Line2D Line { get; }
    }
}
