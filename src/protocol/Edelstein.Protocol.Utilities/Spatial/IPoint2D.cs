using Edelstein.Protocol.Utilities.Spatial.Collections;

namespace Edelstein.Protocol.Utilities.Spatial;

public interface IPoint2D : IObject2D
{
    int X { get; }
    int Y { get; }

    float Distance(IPoint2D point);
}
