using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handlers;

public class CheckSPWRequestHandler : IPacketHandler<ILoginStageUser>
{
    private readonly IPipeline<UserOnPacketCheckSPWRequest> _pipeline;
    
    public short Operation => (short)PacketRecvOperations.CheckSPWRequest;

    public CheckSPWRequestHandler(IPipeline<UserOnPacketCheckSPWRequest> pipeline) => _pipeline = pipeline;

    public bool Check(ILoginStageUser user) =>
        user.State == LoginState.SelectCharacter &&
        user.Account?.SPW != null;

    public Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        var message = new UserOnPacketCheckSPWRequest(
            user,
            reader.ReadString(),
            reader.ReadInt(),
            reader.ReadString(),
            reader.ReadString()
        );

        return _pipeline.Process(message);
    }
}
