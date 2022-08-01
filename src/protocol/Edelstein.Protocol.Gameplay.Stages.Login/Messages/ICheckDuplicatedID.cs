using Edelstein.Protocol.Gameplay.Stages.Messages;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Messages;

public interface ICheckDuplicatedID : IStageUserMessage<ILoginStageUser>
{
    string Name { get; }
}
