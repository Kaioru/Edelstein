using Edelstein.Protocol.Util.Spatial;
using Edelstein.Protocol.Util.Spatial.Collections;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Spatial;

public interface IFieldSpace<TObject> : ISpace2D<TObject> where TObject : IFieldSpaceObject
{
    IReadOnlyCollection<TObject> Objects { get; }
    IRectangle2D Bounds { get; }

    IEnumerable<TObject> FindBelow(IPoint2D point);
    TObject? FindByID(int id);
}
