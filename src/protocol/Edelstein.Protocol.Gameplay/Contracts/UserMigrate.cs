using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Protocol.Gameplay.Contracts;

public record UserMigrate<TStageUser>(
    TStageUser User,
    string ServerID,
    IPacket? Packet
) where TStageUser : IStageUser<TStageUser>;
