using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Packets;

public class UserGatherItemRequestHandler(
    IPipeline<PipedPacketMessage<IGameStageSystem, IGameStageSystemUser, UserGatherItemRequest>> pipeline
) : PipedPacketHandler<IGameStageSystem, IGameStageSystemUser, UserGatherItemRequest>(
    (short)PacketRecvOperation.UserGatherItemRequest,
    pipeline
);
