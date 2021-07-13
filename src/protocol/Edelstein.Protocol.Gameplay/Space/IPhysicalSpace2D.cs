using System.Collections.Generic;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Space
{
    public interface IPhysicalSpace2D
    {
        Rect2D Bounds { get; }

        IPhysicalPoint2D GetPortal(int id);
        ICollection<IPhysicalPoint2D> GetPortals();

        IPhysicalPoint2D GetStartPoint(int id);
        IPhysicalPoint2D GetStartPointClosestTo(Point2D point);
        ICollection<IPhysicalPoint2D> GetStartPoints();

        IPhysicalLine2D GetFoothold(int id);
        IPhysicalLine2D GetFootholdClosestTo(Point2D point);
        IPhysicalLine2D GetFootholdUnderneath(Point2D point);
        ICollection<IPhysicalLine2D> GetFootholds();
    }
}
