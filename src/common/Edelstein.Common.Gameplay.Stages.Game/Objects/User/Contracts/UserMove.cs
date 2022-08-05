using Edelstein.Protocol.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Contracts;

public record UserMove(
    IFieldUser User,
    IMovePath Path
) : IUserMove;
