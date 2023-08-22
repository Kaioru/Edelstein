using Edelstein.Protocol.Utilities.Spatial.Collections;
using RBush;

namespace Edelstein.Common.Utilities.Spatial.Collections;

public class RBushSpaceObject2D<TObject> : ISpatialData where TObject : IObject2D
{
    private readonly Envelope _envelope;

    public RBushSpaceObject2D(TObject obj)
    {
        _envelope = new Envelope(
            obj.MinX,
            obj.MinY,
            obj.MaxX,
            obj.MaxY
        );
        Object = obj;
    }

    public TObject Object { get; }
    public ref readonly Envelope Envelope => ref _envelope;
}
