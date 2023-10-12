using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Packets;

public class UserChangeSlotPositionRequest : AbstractPipedFieldHandler<FieldOnPacketUserChangeSlotPositionRequest>
{

    public UserChangeSlotPositionRequest(IPipeline<FieldOnPacketUserChangeSlotPositionRequest> pipeline) : base(pipeline)
    {
    }
    
    public override short Operation => (short)PacketRecvOperations.UserChangeSlotPositionRequest;

    protected override FieldOnPacketUserChangeSlotPositionRequest? Serialize(IFieldUser user, IPacketReader reader) 
        => new(
            user,
            (ItemInventoryType)reader.Skip(4).ReadByte(),
            reader.ReadShort(),
            reader.ReadShort(),
            reader.ReadShort()
        );
}
