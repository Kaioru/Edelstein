using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Packets;

public class UserChatHandler(
    IPipeline<PipedPacketMessage<IGameStageSystem, IGameStageSystemUser, UserChat>> pipeline
) : PipedPacketHandler<IGameStageSystem, IGameStageSystemUser, UserChat>(
    (short)PacketRecvOperation.UserChat,
    pipeline
);
