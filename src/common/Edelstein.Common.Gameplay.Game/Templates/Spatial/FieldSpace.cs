using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Edelstein.Common.Utilities.Repositories;
using Edelstein.Common.Utilities.Spatial;
using Edelstein.Common.Utilities.Spatial.Collections;
using Edelstein.Protocol.Gameplay.Game.Templates.Spatial;
using Edelstein.Protocol.Utilities.Spatial;
using Edelstein.Protocol.Utilities.Spatial.Collections;

namespace Edelstein.Common.Gameplay.Game.Templates.Spatial;

public class FieldSpace<TObject>(
    IRect2D bounds
) : Repository<int, TObject>,
    IFieldSpace<TObject>
    where TObject : IFieldSpaceObject
{
    private readonly RBushObjectSpace2D<TObject> _space = new();
    public IRect2D Bounds { get; } = bounds;

    public void Insert(IEnumerable<TObject> obj)
    {
        var objects = obj.ToImmutableArray();
        foreach (var o in objects)
            Insert(o);
        _space.Insert(objects);
    }

    public IEnumerable<TObject> Find(IObject2D obj)
        => _space.Find(obj);

    public IEnumerable<TObject> FindClosest(IPoint2D point, int n = 1)
        => _space
            .FindClosest(point, n);

    public IEnumerable<TObject> FindBelow(IPoint2D point)
        => _space
            .Find(new Line2D(point, new Point2D(point.X, Bounds.Bottom)))
            .OrderBy(o => o.MinY);
}
