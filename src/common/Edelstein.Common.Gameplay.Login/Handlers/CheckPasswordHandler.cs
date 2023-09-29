using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handlers;

public class CheckPasswordHandler : AbstractPipedPacketHandler<ILoginStageUser, UserOnPacketCheckPassword>
{    
    public CheckPasswordHandler(IPipeline<UserOnPacketCheckPassword> pipeline) : base(pipeline)
    {
    }
    
    public override short Operation => (short)PacketRecvOperations.CheckPassword;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.CheckPassword;

    public override UserOnPacketCheckPassword Serialize(ILoginStageUser user, IPacketReader reader)
    {
        var username = reader.ReadString();
        var password = reader.ReadString();

        if (user.Context.Options.IsFlippedUsername)
            (username, password) = (password, username);
            
        return new(
            user,
            username,
            password
        );
    }
}
