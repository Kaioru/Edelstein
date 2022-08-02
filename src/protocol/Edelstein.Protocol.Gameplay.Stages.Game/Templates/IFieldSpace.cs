using Edelstein.Protocol.Util.Spatial;
using Edelstein.Protocol.Util.Spatial.Collections;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Templates;

public interface IFieldSpace<TObject> : ISpace2D<TObject> where TObject : IFieldSpaceObject
{
    IRectangle2D Bounds { get; }

    IEnumerable<TObject> FindBelow(IPoint2D point);
    TObject? FindByID(int id);
}
