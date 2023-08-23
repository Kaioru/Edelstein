using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handlers;

public class LogoutWorldHandler : IPacketHandler<ILoginStageUser>
{
    private readonly IPipeline<UserOnPacketLogoutWorld> _pipeline;


    public LogoutWorldHandler(IPipeline<UserOnPacketLogoutWorld> pipeline) => _pipeline = pipeline;
    public short Operation => (short)PacketRecvOperations.LogoutWorld;

    public bool Check(ILoginStageUser user) => user.State == LoginState.SelectCharacter;

    public Task Handle(ILoginStageUser user, IPacketReader reader)
        => _pipeline.Process(new UserOnPacketLogoutWorld(user));
}
