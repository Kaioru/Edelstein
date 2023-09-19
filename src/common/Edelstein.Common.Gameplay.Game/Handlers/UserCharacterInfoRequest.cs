using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class UserCharacterInfoRequest : AbstractPipedFieldHandler<FieldOnPacketUserCharacterInfoRequest>
{
    public UserCharacterInfoRequest(IPipeline<FieldOnPacketUserCharacterInfoRequest> pipeline) : base(pipeline)
    {
    }

    public override short Operation => (short)PacketRecvOperations.UserCharacterInfoRequest;
    
    protected override FieldOnPacketUserCharacterInfoRequest? Serialize(IFieldUser user, IPacketReader reader)
    {
        _ = reader.ReadInt();
        var objID = reader.ReadInt();
        var obj = user.Field?.GetPool(FieldObjectType.User)?.GetObject(objID);

        if (obj is not IFieldUser target) return default;

        return new(user, target);
    }
}
