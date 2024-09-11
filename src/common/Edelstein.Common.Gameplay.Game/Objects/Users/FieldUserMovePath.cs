using Edelstein.Common.Gameplay.Game.Movements;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;

namespace Edelstein.Common.Gameplay.Game.Objects.Users;

public class FieldUserMovePath : AbstractMovePath<IFieldUserMoveAction>, IFieldUserMovePath
{
    protected override IFieldUserMoveAction GetActionFromValue(byte value) => new FieldUserMoveAction(value);
}
