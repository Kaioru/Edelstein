using System.Collections.Generic;
using System.Linq;
using Edelstein.Protocol.Utilities.Spatial;
using Edelstein.Protocol.Utilities.Spatial.Collections;
using RBush;

namespace Edelstein.Common.Utilities.Spatial.Collections;

public class RBushObjectSpace2D<TObject> : IObjectSpace2D<TObject> where TObject : IObject2D
{
    private readonly RBush<RBushObjectSpace2DEntry<TObject>> _tree = new();

    public void Insert(IEnumerable<TObject> obj) 
        => _tree.BulkLoad(obj.Select(o => new RBushObjectSpace2DEntry<TObject>(o)));

    public IEnumerable<TObject> Find(IObject2D obj)
        => _tree
            .Search(new Envelope(obj.MinX, obj.MinY, obj.MaxX, obj.MaxY))
            .Select(o => o.Object);

    public IEnumerable<TObject> FindClosest(IPoint2D point, int n = 1)
        => _tree
            .Knn(n, point.X, point.Y)
            .Select(o => o.Object);
}
