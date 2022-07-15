using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Handlers;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public abstract class AbstractLoginPacketHandler : ILoginPacketHandler
{
    public abstract short Operation { get; }

    public virtual bool Check(ILoginStageUser user) => true;
    public abstract Task Handle(ILoginStageUser user, IPacketReader reader);
}
