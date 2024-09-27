using System;
using Edelstein.Protocol.Gameplay.Game.Movements;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob;

public readonly record struct FieldMobMoveAction : IFieldMobMoveAction
{
    public MoveActionType Type => (MoveActionType)(Value >> 1 & 0x1F);
    public MoveActionDirection Direction => (MoveActionDirection)(Value & 1);
    
    public byte Value { get; init; }
    
    public FieldMobMoveAction(byte value)
        => Value = value;
    
    public FieldMobMoveAction(MoveAbilityType ability, bool isFacingLeft)
    {
        Value = (byte)(
            Convert.ToByte(isFacingLeft) & 1 |
            2 * (byte)(ability switch
                {
                    MoveAbilityType.Fly => MoveActionType.Fly1,
                    MoveAbilityType.Stop => MoveActionType.Stand,
                    _ => MoveActionType.Move
                }
            )
        );
    }
}
