using Edelstein.Protocol.Gameplay.Stages.Game.Movements;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Movements;

public interface INPCMovePath : IMovePath
{
    byte Act { get; }
    byte Chat { get; }

    bool IsMove { get; }
}
