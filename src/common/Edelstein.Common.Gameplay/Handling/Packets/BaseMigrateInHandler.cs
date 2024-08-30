using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Handling.Packets;

public class BaseMigrateInHandler<TStageSystem, TStageSystemUser>(
    IPipeline<PipedPacketMessage<TStageSystem, TStageSystemUser, MigrateIn>> pipeline
) : PipedPacketHandler<TStageSystem, TStageSystemUser, MigrateIn>(
    (short)PacketRecvOperation.MigrateIn,
    pipeline
)
    where TStageSystem : IStageSystem<TStageSystem, TStageSystemUser> 
    where TStageSystemUser : class, IStageSystemUser<TStageSystem, TStageSystemUser>;
