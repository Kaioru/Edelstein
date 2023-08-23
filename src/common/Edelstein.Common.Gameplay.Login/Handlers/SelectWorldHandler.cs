using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handlers;

public class SelectWorldHandler : AbstractPipedPacketHandler<ILoginStageUser, UserOnPacketSelectWorld>
{
    public SelectWorldHandler(IPipeline<UserOnPacketSelectWorld?> pipeline) : base(pipeline)
    {
    }
    
    public override short Operation => (short)PacketRecvOperations.SelectWorld;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.SelectWorld;

    public override UserOnPacketSelectWorld Serialize(ILoginStageUser user, IPacketReader reader)
        => new UserOnPacketSelectWorld(
            user,
            reader.Skip(1).ReadByte(),
            reader.ReadByte()
        );
}
