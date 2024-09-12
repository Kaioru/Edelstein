using Edelstein.Protocol.Gameplay.Game.Movements;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC;

namespace Edelstein.Common.Gameplay.Game.Objects.NPC;

public readonly record struct FieldNPCMoveAction(
    byte Value
) : IFieldNPCMoveAction
{
    public MoveActionType Type => (MoveActionType)(Value >> 1 & 0x1F);
    public MoveActionDirection Direction => (MoveActionDirection)(Value & 1);
}
