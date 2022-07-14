namespace Edelstein.Protocol.Util.Spatial.Collections;

public interface ISpace2D<out TSpaceObject> where TSpaceObject : ISpaceObject2D
{
    TSpaceObject? FindObjectClosest(IObject2D obj);
    TSpaceObject? FindObjectBelow(IObject2D obj);
    TSpaceObject? FindObjectUnderneath(IObject2D obj);
}
