using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Recv;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Packets;

public class DeleteCharacterHandler(
    IPipeline<PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, DeleteCharacter>> pipeline
) : PipedPacketHandler<ILoginStageSystem, ILoginStageSystemUser, DeleteCharacter>(
    (short)PacketRecvOperation.DeleteCharacter,
    pipeline
);
