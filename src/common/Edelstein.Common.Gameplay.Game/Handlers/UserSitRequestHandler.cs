using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class UserSitRequestHandler : AbstractPipedFieldHandler<FieldOnPacketUserSitRequest>
{
    public UserSitRequestHandler(IPipeline<FieldOnPacketUserSitRequest> pipeline) : base(pipeline)
    {
    }

    public override short Operation => (short)PacketRecvOperations.UserSitRequest;

    protected override FieldOnPacketUserSitRequest? Serialize(IFieldUser user, IPacketReader reader)
        => new(user, reader.ReadShort());
}
