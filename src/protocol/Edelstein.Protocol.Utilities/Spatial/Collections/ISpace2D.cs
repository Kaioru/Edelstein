namespace Edelstein.Protocol.Utilities.Spatial.Collections;

public interface ISpace2D<TObject> where TObject : IObject2D
{
    void Insert(IEnumerable<TObject> obj);

    IEnumerable<TObject> Find(IObject2D obj);
    IEnumerable<TObject> FindClosest(IPoint2D point, int n = 1);
}
