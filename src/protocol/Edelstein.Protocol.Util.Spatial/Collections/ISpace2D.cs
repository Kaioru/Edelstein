namespace Edelstein.Protocol.Util.Spatial.Collections;

public interface ISpace2D<out TObject> where TObject : IObject2D
{
    IEnumerable<TObject> Find(IObject2D obj);
    IEnumerable<TObject> FindClosest(IPoint2D point, int n);
}
