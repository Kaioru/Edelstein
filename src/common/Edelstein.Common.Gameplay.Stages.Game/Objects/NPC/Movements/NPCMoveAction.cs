using Edelstein.Protocol.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Movements;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.NPC.Movements;

public readonly struct NPCMoveAction : INPCMoveAction
{
    public NPCMoveAction(byte raw) => Raw = raw;

    public MoveActionType Type => (MoveActionType)((Raw >> 1) & 0x1F);
    public bool IsFacingLeft => (Raw & 1) != 0;

    public byte Raw { get; }
}
