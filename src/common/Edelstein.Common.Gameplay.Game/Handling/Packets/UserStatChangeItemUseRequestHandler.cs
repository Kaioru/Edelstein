using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Packets;

public class UserStatChangeItemUseRequestHandler(
    IPipeline<PipedPacketMessage<IGameStageSystem, IGameStageSystemUser, UserStatChangeItemUseRequest>> pipeline
) : PipedPacketHandler<IGameStageSystem, IGameStageSystemUser, UserStatChangeItemUseRequest>(
    (short)PacketRecvOperation.UserStatChangeItemUseRequest,
    pipeline
);
