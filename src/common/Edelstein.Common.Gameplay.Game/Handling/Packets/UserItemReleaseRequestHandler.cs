using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Packets;

public class UserItemReleaseRequestHandler(
    IPipeline<PipedPacketMessage<IGameStageSystem, IGameStageSystemUser, UserItemReleaseRequest>> pipeline
) : PipedPacketHandler<IGameStageSystem, IGameStageSystemUser, UserItemReleaseRequest>(
    (short)PacketRecvOperation.UserItemReleaseRequest,
    pipeline
);
