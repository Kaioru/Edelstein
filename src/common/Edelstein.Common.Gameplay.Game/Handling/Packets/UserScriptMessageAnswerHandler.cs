using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Packets;

public class UserScriptMessageAnswerHandler(
    IPipeline<PipedPacketMessage<IGameStageSystem, IGameStageSystemUser, UserScriptMessageAnswer>> pipeline
) : PipedPacketHandler<IGameStageSystem, IGameStageSystemUser, UserScriptMessageAnswer>(
    (short)PacketRecvOperation.UserScriptMessageAnswer,
    pipeline
);
