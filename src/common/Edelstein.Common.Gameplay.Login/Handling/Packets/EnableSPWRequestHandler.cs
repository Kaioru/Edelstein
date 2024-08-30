using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Recv;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Packets;

public class EnableSPWRequestHandler(
    IPipeline<PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, EnableSPWRequest>> pipeline
) : PipedPacketHandler<ILoginStageSystem, ILoginStageSystemUser, EnableSPWRequest>(
    (short)PacketRecvOperation.EnableSPWRequest,
    pipeline
);
