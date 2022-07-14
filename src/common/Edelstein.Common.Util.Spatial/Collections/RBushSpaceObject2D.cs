using Edelstein.Protocol.Util.Spatial;
using RBush;

namespace Edelstein.Common.Util.Spatial.Collections;

public class RBushSpaceObject2D<TObject> : ISpatialData where TObject : IObject2D
{
    private readonly Envelope _envelope;
    public ref readonly Envelope Envelope => ref _envelope;

    public TObject Object { get; }

    public RBushSpaceObject2D(TObject obj)
    {
        _envelope = new Envelope(
            MinX: obj.MinX,
            MinY: obj.MinY,
            MaxX: obj.MaxX,
            MaxY: obj.MaxY
        );
        Object = obj;
    }
}
