namespace Edelstein.Protocol.Util.Spatial.Collections;

public interface ISpace2D<out TSpaceObject> where TSpaceObject : ISpaceObject2D
{
    IEnumerable<TSpaceObject> Find(IObject2D obj);
    IEnumerable<TSpaceObject> FindClosest(IPoint2D point, int n);
}
