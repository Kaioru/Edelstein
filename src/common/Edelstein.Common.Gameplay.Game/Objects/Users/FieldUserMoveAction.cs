using Edelstein.Protocol.Gameplay.Game.Movements;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;

namespace Edelstein.Common.Gameplay.Game.Objects.Users;

public readonly record struct FieldUserMoveAction(
    byte Value
) : IFieldUserMoveAction
{
    public MoveActionType Type => (MoveActionType)(Value >> 1 & 0x1F);
    public MoveActionDirection Direction => (MoveActionDirection)(Value & 1);
}
