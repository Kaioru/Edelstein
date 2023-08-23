using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handlers;

public class SelectWorldHandler : IPacketHandler<ILoginStageUser>
{
    private readonly IPipeline<UserOnPacketSelectWorld> _pipeline;


    public SelectWorldHandler(IPipeline<UserOnPacketSelectWorld> pipeline) => _pipeline = pipeline;
    public short Operation => (short)PacketRecvOperations.SelectWorld;

    public bool Check(ILoginStageUser user) => user.State == LoginState.SelectWorld;

    public Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        _ = reader.ReadByte(); // Unknown1

        var message = new UserOnPacketSelectWorld(
            user,
            reader.ReadByte(),
            reader.ReadByte()
        );

        return _pipeline.Process(message);
    }
}
