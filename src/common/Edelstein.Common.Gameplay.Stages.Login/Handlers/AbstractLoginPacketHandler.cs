using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public abstract class AbstractLoginPacketHandler : IPacketHandler<ILoginStageUser>
{
    public abstract short Operation { get; }

    public virtual bool Check(ILoginStageUser user) => true;
    public abstract Task Handle(ILoginStageUser user, IPacketReader reader);
}
