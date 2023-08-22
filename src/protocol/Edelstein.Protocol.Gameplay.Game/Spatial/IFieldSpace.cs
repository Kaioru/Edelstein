using Edelstein.Protocol.Utilities.Spatial;
using Edelstein.Protocol.Utilities.Spatial.Collections;

namespace Edelstein.Protocol.Gameplay.Game.Spatial;

public interface IFieldSpace<TObject> : ISpace2D<TObject> where TObject : IFieldSpaceObject
{
    IRectangle2D Bounds { get; }
    IReadOnlyCollection<TObject> Objects { get; }

    TObject? FindByID(int id);
    IEnumerable<TObject> FindBelow(IPoint2D point);
}
