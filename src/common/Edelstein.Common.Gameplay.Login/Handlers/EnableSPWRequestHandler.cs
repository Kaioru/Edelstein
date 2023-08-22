using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handlers;

public class EnableSPWRequestHandler : IPacketHandler<ILoginStageUser>
{
    private readonly IPipeline<UserOnPacketEnableSPWRequest> _pipeline;
    
    public short Operation => (short)PacketRecvOperations.EnableSPWRequest;

    public EnableSPWRequestHandler(IPipeline<UserOnPacketEnableSPWRequest> pipeline) => _pipeline = pipeline;

    public bool Check(ILoginStageUser user) =>
        user.State == LoginState.SelectCharacter &&
        user.Account?.SPW == null;

    public Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        _ = reader.ReadBool(); // Unknown1

        var message = new UserOnPacketEnableSPWRequest(
            user,
            reader.ReadInt(),
            reader.ReadString(),
            reader.ReadString(),
            reader.ReadString()
        );

        return _pipeline.Process(message);
    }
}
