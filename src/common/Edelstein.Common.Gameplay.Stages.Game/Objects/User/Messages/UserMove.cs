using Edelstein.Protocol.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Messages;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Messages;

public record UserMove(
    IFieldUser User,
    IMovePath Path
) : IUserMove;
