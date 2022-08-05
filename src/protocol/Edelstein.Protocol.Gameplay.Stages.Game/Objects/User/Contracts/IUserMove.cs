using Edelstein.Protocol.Gameplay.Stages.Game.Movements;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts;

public interface IUserMove : IFieldUserMessage
{
    IMovePath Path { get; }
}
