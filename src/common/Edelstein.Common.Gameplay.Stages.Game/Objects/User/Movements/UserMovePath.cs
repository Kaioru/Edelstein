using Edelstein.Common.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Movements;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Movements;

public class UserMovePath : AbstractMovePath<IUserMoveAction>, IUserMovePath
{
    protected override IUserMoveAction GetActionFromRaw(byte raw) => new UserMoveAction(raw);
}
