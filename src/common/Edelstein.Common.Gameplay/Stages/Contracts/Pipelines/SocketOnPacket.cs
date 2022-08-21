using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Contracts.Pipelines;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Contracts.Pipelines;

public record SocketOnPacket<TStageUser>(
    TStageUser User,
    IPacket Packet
) : ISocketOnPacket<TStageUser> where TStageUser : IStageUser<TStageUser>;
