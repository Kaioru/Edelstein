using Edelstein.Protocol.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Spatial;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects;

public interface IFieldLife<in TMovePath> : IFieldObject where TMovePath : IMovePath
{
    byte Action { get; }
    IFieldFoothold? Foothold { get; }

    void SetPosition(IPoint2D position);

    Task Move(IPoint2D position);
    Task Move(TMovePath ctx);
}
