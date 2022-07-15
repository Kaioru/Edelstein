using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Common.Gameplay.Packets;

public interface IPacketHandler<in TStageUser> where TStageUser : IStageUser
{
    short Operation { get; }

    Task Handle(TStageUser user, IPacketReader reader);
}
