using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class UserMigrateToCashShopRequestHandler : AbstractPipedFieldHandler<FieldOnPacketUserMigrateToCashShopRequest>
{
    public UserMigrateToCashShopRequestHandler(IPipeline<FieldOnPacketUserMigrateToCashShopRequest?> pipeline) : base(pipeline)
    {
    }

    public override short Operation => (short)PacketRecvOperations.UserMigrateToCashShopRequest;

    protected override FieldOnPacketUserMigrateToCashShopRequest? Serialize(IFieldUser user, IPacketReader reader)
        => new(user);
}
