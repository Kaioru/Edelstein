using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handlers;

public class CheckPasswordHandler : IPacketHandler<ILoginStageUser>
{
    private readonly IPipeline<UserOnPacketCheckPassword> _pipeline;

    public short Operation => (short)PacketRecvOperations.CheckPassword;
    
    public CheckPasswordHandler(IPipeline<UserOnPacketCheckPassword> pipeline) => _pipeline = pipeline;

    public bool Check(ILoginStageUser user) => user.State == LoginState.CheckPassword;

    public Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        var message = new UserOnPacketCheckPassword(
            user,
            reader.ReadString(),
            reader.ReadString()
        );

        return _pipeline.Process(message);
    }
}
