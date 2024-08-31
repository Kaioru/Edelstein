using Edelstein.Protocol.Utilities.Spatial.Collections;
using RBush;

namespace Edelstein.Common.Utilities.Spatial.Collections;

public record RBushObjectSpace2DEntry<TObject>(
    TObject Object
) : ISpatialData
    where TObject : IObject2D
{
    private readonly Envelope _envelope = new(
        Object.MinX,
        Object.MinY,
        Object.MaxX,
        Object.MaxY
    );
    public ref readonly Envelope Envelope => ref _envelope;
}
