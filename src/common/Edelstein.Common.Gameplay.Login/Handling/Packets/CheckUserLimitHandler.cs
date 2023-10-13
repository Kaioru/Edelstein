using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Handling.Packets;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Packets;

public class CheckUserLimitHandler : AbstractPipedPacketHandler<ILoginStageUser, UserOnPacketCheckUserLimit>
{
    public CheckUserLimitHandler(IPipeline<UserOnPacketCheckUserLimit> pipeline) : base(pipeline)
    {
    }
    
    public override short Operation => (short)PacketRecvOperations.CheckUserLimit;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.SelectWorld;

    public override UserOnPacketCheckUserLimit Serialize(ILoginStageUser user, IPacketReader reader)
        => new(
            user,
            reader.ReadByte()
        );
}
