using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Drop;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Packets;

public class DropPickUpRequestHandler : AbstractPipedFieldHandler<FieldOnPacketDropPickupRequest>
{
    public DropPickUpRequestHandler(IPipeline<FieldOnPacketDropPickupRequest> pipeline) : base(pipeline)
    {
    }
    public override short Operation => (short)PacketRecvOperations.DropPickUpRequest;

    protected override FieldOnPacketDropPickupRequest? Serialize(IFieldUser user, IPacketReader reader)
    {
        _ = reader.ReadByte();
        _ = reader.ReadInt(); // get_update_time
        var position = reader.ReadPoint2D();
        var objID = reader.ReadInt();
        _ = reader.ReadInt(); // crc
        
        var obj = user.Field?.GetPool(FieldObjectType.Drop)?.GetObject(objID);

        if (obj is not IFieldDrop drop) return default;

        return new(
            user,
            drop,
            position
        );
    }
}
