using Edelstein.Protocol.Gameplay.Stages.Contracts;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts;

public interface ICheckUserLimit : IStageUserMessage<ILoginStageUser>
{
    int WorldID { get; }
}
