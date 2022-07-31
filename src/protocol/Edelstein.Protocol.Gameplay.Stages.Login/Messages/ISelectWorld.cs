using Edelstein.Protocol.Gameplay.Stages.Messages;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Messages;

public interface ISelectWorld : IStageUserMessage<ILoginStageUser>
{
    int WorldID { get; }
    int ChannelIndex { get; }
}
