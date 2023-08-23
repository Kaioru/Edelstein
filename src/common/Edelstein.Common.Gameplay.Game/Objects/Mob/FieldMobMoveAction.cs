using Edelstein.Protocol.Gameplay.Game.Movements;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob;

public class FieldMobMoveAction : IFieldMobMoveAction
{

    public FieldMobMoveAction(byte raw) => Raw = raw;
    public FieldMobMoveAction(MobMoveAbilityType ability, bool isFacingLeft) =>
        Raw = (byte)(
            Convert.ToByte(isFacingLeft) & 1 |
            2 * (byte)(ability switch
                {
                    MobMoveAbilityType.Fly => MoveActionType.Fly1,
                    MobMoveAbilityType.Stop => MoveActionType.Stand,
                    _ => MoveActionType.Move
                }
            )
        );
    public MoveActionType Type => (MoveActionType)(Raw >> 1 & 0x1F);
    public MoveActionDirection Direction => (MoveActionDirection)(Raw & 1);

    public byte Raw { get; }
}
