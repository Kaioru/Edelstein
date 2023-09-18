using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handlers;

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
