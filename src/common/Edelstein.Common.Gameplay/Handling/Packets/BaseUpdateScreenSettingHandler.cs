using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Handling.Packets;

public class BaseUpdateScreenSettingHandler<TStageSystem, TStageSystemUser>(
    IPipeline<PipedPacketMessage<TStageSystem, TStageSystemUser, UpdateScreenSetting>> pipeline
) : PipedPacketHandler<TStageSystem, TStageSystemUser, UpdateScreenSetting>(
    (short)PacketRecvOperation.UpdateScreenSetting,
    pipeline
)
    where TStageSystem : IStageSystem<TStageSystem, TStageSystemUser> 
    where TStageSystemUser : IStageSystemUser<TStageSystem, TStageSystemUser>;
