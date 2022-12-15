using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Storages;

namespace Edelstein.Protocol.Gameplay;

public interface IStageUser : IIdentifiable<int>, INetworkAdapter
{
    IStage Stage { get; }
}
