using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Packets;

public class UserTransferFieldRequestHandler : AbstractPipedFieldHandler<FieldOnPacketUserTransferFieldRequest>
{
    public override short Operation => (short)PacketRecvOperations.UserTransferFieldRequest;

    public UserTransferFieldRequestHandler(IPipeline<FieldOnPacketUserTransferFieldRequest> pipeline) : base(pipeline)
    {
    }
    protected override FieldOnPacketUserTransferFieldRequest? Serialize(IFieldUser user, IPacketReader reader)
        => new(
            user,
            reader.Skip(1).ReadInt(),
            reader.ReadString()
        );
}
