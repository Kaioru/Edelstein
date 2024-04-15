using System.Threading.Tasks;
using Edelstein.Protocol.Services.Server;

namespace Edelstein.Protocol.Gameplay;

public interface IStage<TStageSystem, TStageOptions>
    where TStageSystem : IStageSystem<TStageOptions> 
    where TStageOptions : IServerEntry
{
    TStageSystem System { get; }

    Task Enter(IStageUser<TStageSystem, TStageOptions> user);
    Task Leave(IStageUser<TStageSystem, TStageOptions> user);
}
