using Edelstein.Protocol.Services.Server;

namespace Edelstein.Protocol.Gameplay;

public interface IStageSystem<out TStageOptions>
    where TStageOptions : IServerEntry
{
    TStageOptions Options { get; }
}
