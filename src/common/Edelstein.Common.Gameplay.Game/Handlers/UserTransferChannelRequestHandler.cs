using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class UserTransferChannelRequestHandler : AbstractPipedFieldHandler<FieldOnPacketUserTransferChannelRequest>
{
    public UserTransferChannelRequestHandler(IPipeline<FieldOnPacketUserTransferChannelRequest> pipeline) : base(pipeline)
    {
    }

    public override short Operation => (short)PacketRecvOperations.UserTransferChannelRequest;

    protected override FieldOnPacketUserTransferChannelRequest? Serialize(IFieldUser user, IPacketReader reader)
        => new(user, reader.ReadByte());
}
