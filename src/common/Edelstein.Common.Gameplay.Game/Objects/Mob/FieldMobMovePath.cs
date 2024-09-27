using Edelstein.Common.Gameplay.Game.Movements;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob;

public class FieldMobMovePath : AbstractMovePath<IFieldMobMoveAction>, IFieldMobMovePath
{
    protected override IFieldMobMoveAction GetActionFromValue(byte value) => new FieldMobMoveAction(value);
}
