using Edelstein.Protocol.Gameplay.Stages.Game.Movements;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Messages;

public interface IUserMove : IFieldUserMessage
{
    IMovePath Path { get; }
}
