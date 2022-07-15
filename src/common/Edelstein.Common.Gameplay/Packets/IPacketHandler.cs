using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Common.Gameplay.Packets;

public interface IPacketHandler<in TStageUser> where TStageUser : IStageUser
{
    short Operation { get; }

    bool Check(TStageUser user);

    Task Handle(TStageUser user, IPacketReader reader);
}
