using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Messages;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Common.Gameplay.Stages.Messages;

public record StageUserOnPacket<TStageUser>(
    TStageUser User,
    IPacketReader Packet
) : IStageUserOnPacket<TStageUser> where TStageUser : IStageUser;
