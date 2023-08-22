using Edelstein.Protocol.Gameplay.Game.Movements;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Objects.User;

public readonly struct FieldUserMoveAction : IFieldUserMoveAction
{
    public FieldUserMoveAction(byte raw) => Raw = raw;

    public MoveActionType Type => (MoveActionType)((Raw >> 1) & 0x1F);
    public MoveActionDirection Direction => (MoveActionDirection)(Raw & 1);

    public byte Raw { get; }
}
