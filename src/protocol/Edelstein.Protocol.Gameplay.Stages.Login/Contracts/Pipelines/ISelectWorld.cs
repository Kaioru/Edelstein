using Edelstein.Protocol.Gameplay.Stages.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Contracts.Pipelines;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;

public interface ISelectWorld : IStageUserContract<ILoginStageUser>
{
    int WorldID { get; }
    int ChannelID { get; }
}
