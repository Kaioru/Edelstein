using System.Collections.Generic;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Spatial
{
    public interface IPhysicalCollection2D<TObject> where TObject : class, IPhysicalObject2D
    {
        void Insert(TObject obj);
        void Insert(IEnumerable<TObject> objs);

        TObject Find(int id);
        TObject FindRandom();
        TObject FindNearest(Point2D point);
        TObject FindNearestBelow(Point2D point);
    }
}
