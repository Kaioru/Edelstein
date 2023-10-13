using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Handling.Packets;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Packets;

public class EnableSPWRequestHandler : AbstractPipedPacketHandler<ILoginStageUser, UserOnPacketEnableSPWRequest>
{
    public EnableSPWRequestHandler(IPipeline<UserOnPacketEnableSPWRequest> pipeline) : base(pipeline)
    {
    }
    
    public override short Operation => (short)PacketRecvOperations.EnableSPWRequest;

    public override bool Check(ILoginStageUser user) =>
        user.State == LoginState.SelectCharacter &&
        user.Account?.SPW == null;

    public override UserOnPacketEnableSPWRequest Serialize(ILoginStageUser user, IPacketReader reader)
        => new(
            user,
            reader.Skip(1).ReadInt(),
            reader.ReadString(),
            reader.ReadString(),
            reader.ReadString()
        );
}
