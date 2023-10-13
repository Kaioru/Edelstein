using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Packets;

public class UserPortableChairSitRequestHandler : AbstractPipedFieldHandler<FieldOnPacketUserPortableChairSitRequest>
{
    public UserPortableChairSitRequestHandler(IPipeline<FieldOnPacketUserPortableChairSitRequest> pipeline) : base(pipeline)
    {
    }

    public override short Operation => (short)PacketRecvOperations.UserPortableChairSitRequest;

    protected override FieldOnPacketUserPortableChairSitRequest? Serialize(IFieldUser user, IPacketReader reader)
        => new(user, reader.ReadInt());
}
