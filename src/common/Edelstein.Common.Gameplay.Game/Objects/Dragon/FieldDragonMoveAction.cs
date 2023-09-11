using Edelstein.Protocol.Gameplay.Game.Movements;
using Edelstein.Protocol.Gameplay.Game.Objects.Dragon;

namespace Edelstein.Common.Gameplay.Game.Objects.Dragon;

public class FieldDragonMoveAction : IFieldDragonMoveAction
{
    public FieldDragonMoveAction(byte raw) => Raw = raw;

    public MoveActionType Type => (MoveActionType)(Raw >> 1 & 0x1F);
    public MoveActionDirection Direction => (MoveActionDirection)(Raw & 1);

    public byte Raw { get; }
}
