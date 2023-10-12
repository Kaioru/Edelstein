using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Handling.Packets;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Packets;

public class SelectWorldHandler : AbstractPipedPacketHandler<ILoginStageUser, UserOnPacketSelectWorld>
{
    public SelectWorldHandler(IPipeline<UserOnPacketSelectWorld> pipeline) : base(pipeline)
    {
    }
    
    public override short Operation => (short)PacketRecvOperations.SelectWorld;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.SelectWorld;

    public override UserOnPacketSelectWorld Serialize(ILoginStageUser user, IPacketReader reader)
        => new(
            user,
            reader.Skip(1).ReadByte(),
            reader.ReadByte()
        );
}
