using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handlers;

public class WorldInfoRequestHandler : AbstractPipedPacketHandler<ILoginStageUser, UserOnPacketWorldRequest>
{
    public WorldInfoRequestHandler(IPipeline<UserOnPacketWorldRequest?> pipeline) : base(pipeline)
    {
    }
    
    public override short Operation => (short)PacketRecvOperations.WorldInfoRequest;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.SelectWorld;

    public override UserOnPacketWorldRequest Serialize(ILoginStageUser user, IPacketReader reader)
        => new(user);
}
