namespace Edelstein.Protocol.Util.Spatial.Collections;

public interface ISpace2D<TObject> where TObject : IObject2D
{
    void BulkInsert(IEnumerable<TObject> obj);

    IEnumerable<TObject> Find(IObject2D obj);
    IEnumerable<TObject> FindClosest(IPoint2D point, int n);
}
