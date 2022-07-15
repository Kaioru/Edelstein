namespace Edelstein.Protocol.Util.Spatial;

public interface IRectangle2D : IObject2D
{
    IPoint2D P1 { get; }
    IPoint2D P2 { get; }
    
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
