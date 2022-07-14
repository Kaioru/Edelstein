namespace Edelstein.Protocol.Util.Spatial.Collections;

public interface ISpace2D<out TSpaceObject> where TSpaceObject : ISpaceObject2D
{
    IEnumerable<TSpaceObject> FindObject(IObject2D obj);
    IEnumerable<TSpaceObject> FindObjectClosest(IObject2D obj, int n);
}
