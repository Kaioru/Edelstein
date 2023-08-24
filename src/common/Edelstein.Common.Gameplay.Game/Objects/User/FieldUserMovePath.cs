using Edelstein.Common.Gameplay.Game.Movements;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Objects.User;

public class FieldUserMovePath : AbstractMovePath<IFieldUserMoveAction>, IFieldUserMovePath
{
    protected override IFieldUserMoveAction GetActionFromRaw(byte raw) => new FieldUserMoveAction(raw);
}
