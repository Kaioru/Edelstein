using System.Collections.Generic;
using Edelstein.Protocol.Utilities.Repositories.Methods;
using Edelstein.Protocol.Utilities.Spatial;
using Edelstein.Protocol.Utilities.Spatial.Collections;

namespace Edelstein.Protocol.Gameplay.Game.Templates.Spatial;

public interface IFieldSpace<TObject> : 
    IObjectSpace2D<TObject>,
    IRepositoryMethodRetrieve<int, TObject>,
    IRepositoryMethodRetrieveAll<int, TObject>
    where TObject : IFieldSpaceObject
{
    IRect2D Bounds { get; }
    
    IEnumerable<TObject> FindBelow(IPoint2D point);
}
