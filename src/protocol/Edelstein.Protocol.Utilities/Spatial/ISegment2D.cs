using Edelstein.Protocol.Utilities.Spatial.Collections;

namespace Edelstein.Protocol.Utilities.Spatial;

public interface ISegment2D : IObject2D
{
    IPoint2D P1 { get; }
    IPoint2D P2 { get; }

    IPoint2D Middle { get; }

    float Length { get; }
    float? Slope { get; }

    bool IsVertical { get; }
    bool IsHorizontal { get; }
    bool IsAbove(IPoint2D point);
    bool IsBelow(IPoint2D point);

    IPoint2D? AtX(int x);
    IPoint2D? AtY(int y);

    bool Intersects(IPoint2D point);
}
