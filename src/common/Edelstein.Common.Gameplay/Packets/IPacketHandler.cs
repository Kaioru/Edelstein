using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Packets;

public interface IPacketHandler<in TStageUser> where TStageUser : IStageUser<TStageUser>
{
    short Operation { get; }

    bool Check(TStageUser user);

    Task Handle(TStageUser user, IPacketReader reader);
}
