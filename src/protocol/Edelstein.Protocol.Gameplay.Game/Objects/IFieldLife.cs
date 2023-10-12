using Edelstein.Protocol.Gameplay.Game.Movements;
using Edelstein.Protocol.Gameplay.Game.Spatial;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Objects;

public interface IFieldLife<in TMovePath, out TMoveAction> : IFieldObject
    where TMovePath : IMovePath<TMoveAction>
    where TMoveAction : IMoveAction
{
    TMoveAction Action { get; }
    IFieldFoothold? Foothold { get; }

    Task Move(IPoint2D position, IFieldFoothold? foothold, bool init = false);
    Task Move(TMovePath ctx, IFieldObject? controller = null);
}
