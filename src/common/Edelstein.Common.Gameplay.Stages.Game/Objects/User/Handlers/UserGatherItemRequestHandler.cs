using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Contracts;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;
using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers;

public class UserGatherItemRequestHandler : AbstractFieldUserHandler
{
    private readonly IPipeline<IUserGatherItemRequest> _pipeline;

    public UserGatherItemRequestHandler(IPipeline<IUserGatherItemRequest> pipeline) => _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.UserGatherItemRequest;

    protected override Task Handle(IFieldUser user, IPacketReader reader)
    {
        _ = reader.ReadInt();

        var message = new UserGatherItemRequest(
            user,
            (ItemInventoryType)reader.ReadByte()
        );

        return _pipeline.Process(message);
    }
}
