using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Handling;

public interface IPacketHandlerManager<TStageUser> where TStageUser : IStageUser<TStageUser>
{
    void Add(IPacketHandler<TStageUser> handler);
    void Remove(IPacketHandler<TStageUser> handler);

    Task Process(TStageUser user, IPacket packet);
}
