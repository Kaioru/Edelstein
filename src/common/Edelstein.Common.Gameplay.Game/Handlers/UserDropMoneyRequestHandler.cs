using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class UserDropMoneyRequestHandler : AbstractPipedFieldHandler<FieldOnPacketUserDropMoneyRequest>
{
    public override short Operation => (short)PacketRecvOperations.UserDropMoneyRequest;
    
    public UserDropMoneyRequestHandler(IPipeline<FieldOnPacketUserDropMoneyRequest?> pipeline) : base(pipeline)
    {
    }
    
    protected override FieldOnPacketUserDropMoneyRequest? Serialize(IFieldUser user, IPacketReader reader)
        => new(user, reader.Skip(4).ReadInt());
}
