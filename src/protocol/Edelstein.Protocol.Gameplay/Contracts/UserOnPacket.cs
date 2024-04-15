using Edelstein.Protocol.Utilities.Buffers;

namespace Edelstein.Protocol.Gameplay.Contracts;

public record UserOnPacket<TStageUser, TStageSystem>(
    TStageUser User,
    IPacket Packet
) 
    where TStageUser : IStageUser<TStageUser, TStageSystem> 
    where TStageSystem : IStageSystem<TStageUser, TStageSystem>;
