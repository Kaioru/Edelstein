using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Space
{
    public interface IPhysicalSpace2D
    {
        Rect2D Bounds { get; }

        IPhysicalPoint2D GetPortal(int id);
        IPhysicalPoint2D GetPortalClosestTo(Point2D point);

        IPhysicalPoint2D GetStartPointClosestTo(Point2D point);

        IPhysicalLine2D GetFoothold(int id);
        IPhysicalLine2D GetFootholdClosestTo(Point2D point);
        IPhysicalLine2D GetFootholdUnderneath(Point2D point);
    }
}
