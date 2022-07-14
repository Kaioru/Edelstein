namespace Edelstein.Protocol.Util.Spatial;

public interface IPoint2D : IObject2D
{
    int X { get; }
    int Y { get; }

    double Distance(IPoint2D point);
}
