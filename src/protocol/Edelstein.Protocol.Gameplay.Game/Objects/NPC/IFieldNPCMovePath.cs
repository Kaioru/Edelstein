using Edelstein.Protocol.Gameplay.Game.Movements;

namespace Edelstein.Protocol.Gameplay.Game.Objects.NPC;

public interface IFieldNPCMovePath : IMovePath<IFieldNPCMoveAction>
{
    byte Act { get; }
    byte Chat { get; }

    bool IsMove { get; }
}
