using Edelstein.Protocol.Utilities.Spatial.Collections;

namespace Edelstein.Protocol.Utilities.Spatial;

public interface IRectangle2D : IObject2D
{
    IPoint2D P1 { get; }
    IPoint2D P2 { get; }
    
    IPoint2D Center { get; }

    int Left { get; }
    int Right { get; }
    int Top { get; }
    int Bottom { get; }

    float Height { get; }
    float Width { get; }
    float Area { get; }

    bool IsSquare { get; }

    bool Intersects(IPoint2D point);
}
