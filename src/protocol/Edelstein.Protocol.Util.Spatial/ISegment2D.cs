namespace Edelstein.Protocol.Util.Spatial;

public interface ISegment2D : IObject2D
{
    IPoint2D P1 { get; }
    IPoint2D P2 { get; }
    
    IPoint2D Middle { get; }
    
    double Length { get; }
    double Slope { get; }
    
    bool IsVertical { get; }
    bool IsHorizontal { get; }
    bool IsAbove(IPoint2D point);
    bool IsBelow(IPoint2D point);
    
    bool Intersects(IPoint2D point);
}
