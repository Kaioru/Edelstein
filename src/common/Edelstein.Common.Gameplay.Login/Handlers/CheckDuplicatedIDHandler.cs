using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handlers;

public class CheckDuplicatedIDHandler : IPacketHandler<ILoginStageUser>
{
    private readonly IPipeline<UserOnPacketCheckDuplicatedID> _pipeline;

    public CheckDuplicatedIDHandler(IPipeline<UserOnPacketCheckDuplicatedID> pipeline) => _pipeline = pipeline;
    public short Operation => (short)PacketRecvOperations.CheckDuplicatedID;

    public bool Check(ILoginStageUser user) => user.State == LoginState.SelectCharacter;

    public Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        var message = new UserOnPacketCheckDuplicatedID(
            user,
            reader.ReadString()
        );

        return _pipeline.Process(message);
    }
}
