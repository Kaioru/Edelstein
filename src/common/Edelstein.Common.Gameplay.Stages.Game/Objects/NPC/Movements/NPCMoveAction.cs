using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Movements;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.NPC.Movements;

public readonly struct NPCMoveAction : INPCMoveAction
{
    public NPCMoveAction(byte raw) => Raw = raw;

    public byte Raw { get; }
}
