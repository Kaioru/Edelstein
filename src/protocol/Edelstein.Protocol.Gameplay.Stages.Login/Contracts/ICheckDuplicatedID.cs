using Edelstein.Protocol.Gameplay.Stages.Contracts;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts;

public interface ICheckDuplicatedID : IStageUserMessage<ILoginStageUser>
{
    string Name { get; }
}
