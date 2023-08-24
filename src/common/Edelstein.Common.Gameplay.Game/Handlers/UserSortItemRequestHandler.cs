using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class UserSortItemRequestHandler : AbstractPipedFieldHandler<FieldOnPacketUserSortItemRequest>
{

    public UserSortItemRequestHandler(IPipeline<FieldOnPacketUserSortItemRequest?> pipeline) : base(pipeline)
    {
    }

    public override short Operation => (short)PacketRecvOperations.UserSortItemRequest;

    protected override FieldOnPacketUserSortItemRequest? Serialize(IFieldUser user, IPacketReader reader)
        => new(
            user,
            (ItemInventoryType)reader.Skip(4).ReadByte()
        );
}
