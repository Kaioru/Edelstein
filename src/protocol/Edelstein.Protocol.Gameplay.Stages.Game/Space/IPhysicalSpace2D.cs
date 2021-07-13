using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Space
{
    public interface IPhysicalSpace2D
    {
        Rect2D Bounds { get; }

        IPhysicalSpawnPoint2D GetSpawnPoint(int id);
        IPhysicalSpawnPoint2D GetSpawnPointClosestTo(Point2D point);

        IPhysicalFoothold2D GetFoothold(int id);
        IPhysicalFoothold2D GetFootholdClosestTo(Point2D point);
        IPhysicalFoothold2D GetFootholdUnderneath(Point2D point);
    }
}
