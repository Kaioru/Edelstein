using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Movements;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Contracts;

public record UserMove(
    IFieldUser User,
    IUserMovePath Path
) : IUserMove;
