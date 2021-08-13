using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Movements
{
    public interface IMovePath
    {
        MoveActionType? Action { get; }
        Point2D? Position { get; }
        short? FootholdID { get; }
    }
}
