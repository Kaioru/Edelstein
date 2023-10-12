using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Handling.Packets;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Packets;

public class CreateSecurityHandleHandler : AbstractPipedPacketHandler<ILoginStageUser, UserOnPacketCreateSecurityHandle>
{
    public CreateSecurityHandleHandler(IPipeline<UserOnPacketCreateSecurityHandle> pipeline) : base(pipeline)
    {
    }

    public override short Operation => (short)PacketRecvOperations.CreateSecurityHandle;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.CheckPassword;

    public override UserOnPacketCreateSecurityHandle? Serialize(ILoginStageUser user, IPacketReader reader)
        => new(user);
}
