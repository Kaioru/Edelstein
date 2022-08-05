using Edelstein.Protocol.Gameplay.Stages.Contracts;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts;

public interface ISelectWorld : IStageUserMessage<ILoginStageUser>
{
    int WorldID { get; }
    int ChannelID { get; }
}
