using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Movements;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;

public record UserMove(
    IFieldUser User,
    IUserMovePath Path
) : IUserMove;
