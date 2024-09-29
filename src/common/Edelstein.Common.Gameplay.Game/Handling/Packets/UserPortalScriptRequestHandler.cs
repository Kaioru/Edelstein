using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Packets;

public class UserPortalScriptRequestHandler(
    IPipeline<PipedPacketMessage<IGameStageSystem, IGameStageSystemUser, UserPortalScriptRequest>> pipeline
) : PipedPacketHandler<IGameStageSystem, IGameStageSystemUser, UserPortalScriptRequest>(
    (short)PacketRecvOperation.UserPortalScriptRequest,
    pipeline
);
