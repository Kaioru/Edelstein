using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Movements;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Movements;

public readonly struct UserMoveAction : IUserMoveAction
{
    public UserMoveAction(byte raw) => Raw = raw;

    public byte Raw { get; }
}
