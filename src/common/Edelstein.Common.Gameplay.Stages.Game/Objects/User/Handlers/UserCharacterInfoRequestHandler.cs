using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers;

public class UserCharacterInfoRequestHandler : AbstractFieldUserHandler
{
    private readonly IPipeline<IUserCharacterInfoRequest> _pipeline;

    public UserCharacterInfoRequestHandler(IPipeline<IUserCharacterInfoRequest> pipeline) => _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.UserCharacterInfoRequest;

    protected override Task Handle(IFieldUser user, IPacketReader reader)
    {
        _ = reader.ReadInt();

        var message = new UserCharacterInfoRequest(user, reader.ReadInt());

        return _pipeline.Process(message);
    }
}
