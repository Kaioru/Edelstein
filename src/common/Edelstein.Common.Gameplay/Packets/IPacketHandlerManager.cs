using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Util.Buffers.Bytes;

namespace Edelstein.Common.Gameplay.Packets;

public interface IPacketHandlerManager<TStageUser> where TStageUser : IStageUser<TStageUser>
{
    void Add(IPacketHandler<TStageUser> handler);
    void Remove(IPacketHandler<TStageUser> handler);

    Task Process(TStageUser user, IPacket packet);
}
