using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Movements;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;

public interface IUserMove : IFieldUserContract
{
    IUserMovePath Path { get; }
}
