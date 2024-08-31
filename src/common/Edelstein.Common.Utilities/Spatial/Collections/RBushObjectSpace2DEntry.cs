using Edelstein.Protocol.Utilities.Spatial.Collections;
using RBush;

namespace Edelstein.Common.Utilities.Spatial.Collections;

public class RBushObjectSpace2DEntry<TObject>(
    TObject obj
) : ISpatialData
    where TObject : IObject2D
{
    private readonly Envelope _envelope = new(
        obj.MinX,
        obj.MinY,
        obj.MaxX,
        obj.MaxY
    );
    
    public ref readonly Envelope Envelope => ref _envelope;
    public TObject Object { get; } = obj;

}
