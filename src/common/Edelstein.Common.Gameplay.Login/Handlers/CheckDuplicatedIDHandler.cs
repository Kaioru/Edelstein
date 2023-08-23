using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handlers;

public class CheckDuplicatedIDHandler : AbstractPipedPacketHandler<ILoginStageUser, UserOnPacketCheckDuplicatedID>
{
    public CheckDuplicatedIDHandler(IPipeline<UserOnPacketCheckDuplicatedID?> pipeline) : base(pipeline)
    {
    }
    
    public override short Operation => (short)PacketRecvOperations.CheckDuplicatedID;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.SelectCharacter;

    public override UserOnPacketCheckDuplicatedID Serialize(ILoginStageUser user, IPacketReader reader) 
        => new(
            user,
            reader.ReadString()
        );
}
