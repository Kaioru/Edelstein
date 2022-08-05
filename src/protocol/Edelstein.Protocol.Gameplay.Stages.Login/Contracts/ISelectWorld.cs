using Edelstein.Protocol.Gameplay.Stages.Contracts;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts;

public interface ISelectWorld : IStageUserContract<ILoginStageUser>
{
    int WorldID { get; }
    int ChannelID { get; }
}
