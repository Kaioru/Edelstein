using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Handling.Packets;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Packets;

public class CheckSPWRequestHandler : AbstractPipedPacketHandler<ILoginStageUser, UserOnPacketCheckSPWRequest>
{
    public CheckSPWRequestHandler(IPipeline<UserOnPacketCheckSPWRequest> pipeline) : base(pipeline)
    {
    }
    
    public override short Operation => (short)PacketRecvOperations.CheckSPWRequest;

    public override bool Check(ILoginStageUser user) =>
        user is { State: LoginState.SelectCharacter, Account.SPW: not null };

    public override UserOnPacketCheckSPWRequest Serialize(ILoginStageUser user, IPacketReader reader)
        => new(
            user,
            reader.ReadString(),
            reader.ReadInt(),
            reader.ReadString(),
            reader.ReadString()
        );
}
