using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Movements;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Contracts;

public interface INPCMove : IFieldNPCContract
{
    INPCMovePath Path { get; }
}
