using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handlers;

public class CheckUserLimitHandler : IPacketHandler<ILoginStageUser>
{
    private readonly IPipeline<UserOnPacketCheckUserLimit> _pipeline;

    public CheckUserLimitHandler(IPipeline<UserOnPacketCheckUserLimit> pipeline) => _pipeline = pipeline;
    public short Operation => (short)PacketRecvOperations.CheckUserLimit;

    public bool Check(ILoginStageUser user) => user.State == LoginState.SelectWorld;

    public Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        var message = new UserOnPacketCheckUserLimit(
            user,
            reader.ReadByte()
        );

        return _pipeline.Process(message);
    }
}
