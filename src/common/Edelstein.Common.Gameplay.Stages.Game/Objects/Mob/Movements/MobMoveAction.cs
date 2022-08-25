using Edelstein.Protocol.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Templates;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.Mob.Movements;

public struct MobMoveAction : IMobMoveAction
{
    public MobMoveAction(byte raw) => Raw = raw;

    public MobMoveAction(MoveAbilityType ability, bool isFacingLeft) =>
        Raw = (byte)(
            (Convert.ToByte(isFacingLeft) & 1) |
            (2 * (byte)(ability switch
                    {
                        MoveAbilityType.Fly => MoveActionType.Fly1,
                        MoveAbilityType.Stop => MoveActionType.Stand,
                        _ => MoveActionType.Move
                    }
                ))
        );

    public MoveActionType Type => (MoveActionType)((Raw >> 1) & 0x1F);
    public MoveActionDirection Direction => (MoveActionDirection)(Raw & 1);

    public byte Raw { get; }
}
