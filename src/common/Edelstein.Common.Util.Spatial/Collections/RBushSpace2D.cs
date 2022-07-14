using Edelstein.Protocol.Util.Spatial;
using Edelstein.Protocol.Util.Spatial.Collections;
using RBush;

namespace Edelstein.Common.Util.Spatial.Collections;

public class RBushSpace2D<TObject> : ISpace2D<TObject> where TObject : IObject2D
{
    private readonly RBush<RBushSpaceObject2D<TObject>> _tree;

    public RBushSpace2D()
        => _tree = new RBush<RBushSpaceObject2D<TObject>>();

    public void BulkInsert(IEnumerable<TObject> obj)
        => _tree.BulkLoad(obj.Select(o => new RBushSpaceObject2D<TObject>(o)));

    public IEnumerable<TObject> Find(IObject2D obj)
        => _tree
            .Search(new(obj.MinX, obj.MinY, obj.MaxX, obj.MaxY))
            .Select(o => o.Object);

    public IEnumerable<TObject> FindClosest(IPoint2D point, int n)
        => _tree
            .Knn(n, point.X, point.Y)
            .Select(o => o.Object);
}
