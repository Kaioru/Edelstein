using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Handling.Packets;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Packets;

public class CheckPasswordHandler : AbstractPipedPacketHandler<ILoginStageUser, UserOnPacketCheckPassword>
{    
    public CheckPasswordHandler(IPipeline<UserOnPacketCheckPassword> pipeline) : base(pipeline)
    {
    }
    
    public override short Operation => (short)PacketRecvOperations.CheckPassword;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.CheckPassword;

    public override UserOnPacketCheckPassword Serialize(ILoginStageUser user, IPacketReader reader)
        => new(
            user,
            reader.ReadString(),
            reader.ReadString()
        );
}
