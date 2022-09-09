using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Handlers;

public class AbstractPrivateServerPacketHandler<TStageUser> : IPacketHandler<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
    public short Operation => (short)PacketRecvOperations.PrivateServerPacket;

    public bool Check(TStageUser user) => !user.IsMigrating;

    public Task Handle(TStageUser user, IPacketReader reader) =>
        user.Dispatch(
            new PacketWriter(PacketSendOperations.PrivateServerPacket)
                .WriteInt((int)PacketSendOperations.PrivateServerPacket ^ reader.ReadInt())
        );
}
