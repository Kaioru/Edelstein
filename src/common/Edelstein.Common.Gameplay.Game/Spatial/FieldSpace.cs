using System.Collections.Immutable;
using Edelstein.Common.Utilities.Spatial;
using Edelstein.Common.Utilities.Spatial.Collections;
using Edelstein.Protocol.Gameplay.Game.Spatial;
using Edelstein.Protocol.Utilities.Spatial;
using Edelstein.Protocol.Utilities.Spatial.Collections;

namespace Edelstein.Common.Gameplay.Game.Spatial;

public class FieldSpace<TObject> : IFieldSpace<TObject> where TObject : IFieldSpaceObject
{
    private readonly IDictionary<int, TObject> _objects;
    private readonly ISpace2D<TObject> _space;
    
    public IReadOnlyCollection<TObject> Objects => _objects.Values.ToImmutableList();
    public IRectangle2D Bounds { get; }

    public FieldSpace(IRectangle2D bounds)
    {
        Bounds = bounds;
        _objects = new Dictionary<int, TObject>();
        _space = new RBushSpace2D<TObject>();
    }


    public void Insert(IEnumerable<TObject> obj)
    {
        var objects = obj.ToImmutableList();
        foreach (var o in objects)
            _objects.Add(o.ID, o);
        _space.Insert(objects);
    }
    
    public TObject? FindByID(int id) => _objects.TryGetValue(id, out var obj)
        ? obj
        : default;
    
    public IEnumerable<TObject> Find(IObject2D obj) => _space.Find(obj);
    
    public IEnumerable<TObject> FindClosest(IPoint2D point, int n = 1) => _space.FindClosest(point, n);

    public IEnumerable<TObject> FindBelow(IPoint2D point) => _space.Find(
        new Segment2D(point, new Point2D(point.X, Bounds.Bottom))
    );
}
