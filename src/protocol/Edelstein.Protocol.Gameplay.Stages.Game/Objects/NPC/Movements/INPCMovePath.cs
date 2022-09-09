using Edelstein.Protocol.Gameplay.Stages.Game.Movements;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Movements;

public interface INPCMovePath : IMovePath<INPCMoveAction>
{
    byte Act { get; }
    byte Chat { get; }
    int Duration { get; }

    bool IsMove { get; }
}
