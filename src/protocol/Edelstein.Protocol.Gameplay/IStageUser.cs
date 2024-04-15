using Edelstein.Protocol.Network;
using Edelstein.Protocol.Services.Server;

namespace Edelstein.Protocol.Gameplay;

public interface IStageUser<TStageSystem, TStageOptions> : IAdapter
    where TStageSystem : IStageSystem<TStageOptions> 
    where TStageOptions : IServerEntry
{
    TStageSystem System { get; }
    IStage<TStageSystem, TStageOptions>? Stage { get; set; }
}
