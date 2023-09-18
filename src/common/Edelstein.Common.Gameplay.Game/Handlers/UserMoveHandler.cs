using Edelstein.Common.Gameplay.Game.Objects.User;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class UserMoveHandler : AbstractPipedFieldHandler<FieldOnPacketUserMove>
{
    public UserMoveHandler(IPipeline<FieldOnPacketUserMove> pipeline) : base(pipeline)
    {
    }
    
    public override short Operation => (short)PacketRecvOperations.UserMove;

    protected override FieldOnPacketUserMove? Serialize(IFieldUser user, IPacketReader reader)
        => new(user, reader.Skip(29).Read(new FieldUserMovePath()));
}
