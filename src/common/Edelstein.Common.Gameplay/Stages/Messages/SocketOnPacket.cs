using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Messages;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Common.Gameplay.Stages.Messages;

public record SocketOnPacket<TStageUser>(
    TStageUser User,
    IPacketReader Packet
) : ISocketOnPacket<TStageUser> where TStageUser : IStageUser;
