using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Contracts;
using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers;

public class UserSortItemRequestHandler : AbstractFieldUserHandler
{
    private readonly IPipeline<IUserSortItemRequest> _pipeline;

    public UserSortItemRequestHandler(IPipeline<IUserSortItemRequest> pipeline) => _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.UserSortItemRequest;

    protected override Task Handle(IFieldUser user, IPacketReader reader)
    {
        _ = reader.ReadInt();

        var message = new UserSortItemRequest(
            user,
            (ItemInventoryType)reader.ReadByte()
        );

        return _pipeline.Process(message);
    }
}
