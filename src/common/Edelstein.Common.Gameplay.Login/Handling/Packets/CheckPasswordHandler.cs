using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Recv;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Packets;

[PacketHandler(PacketRecvOperation.CheckPassword)]
public class CheckPasswordHandler(
    IPipeline<PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, CheckPassword>> pipeline
) : PipedPacketHandler<ILoginStageSystem, ILoginStageSystemUser, CheckPassword>(
    pipeline
);
