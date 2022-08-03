using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Messages;
using Edelstein.Protocol.Gameplay.Stages.Game.Movements;

namespace Edelstein.Common.Gameplay.Stages.Game.Messages;

public record UserMove(
    IFieldUser User,
    IMovePath Path
) : IUserMove;
