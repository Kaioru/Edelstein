namespace Edelstein.Protocol.Util.Spatial;

public interface IPoint2D : IObject2D
{
    float X { get; }
    float Y { get; }

    float Distance(IPoint2D point);
}
