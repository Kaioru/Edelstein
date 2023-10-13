using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Handling.Packets;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Packets;

public class WorldRequestHandler : AbstractPipedPacketHandler<ILoginStageUser, UserOnPacketWorldRequest>
{
    public WorldRequestHandler(IPipeline<UserOnPacketWorldRequest> pipeline) : base(pipeline)
    {
    }
    
    public override short Operation => (short)PacketRecvOperations.WorldRequest;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.SelectWorld;

    public override UserOnPacketWorldRequest Serialize(ILoginStageUser user, IPacketReader reader) 
        => new(user);
}
