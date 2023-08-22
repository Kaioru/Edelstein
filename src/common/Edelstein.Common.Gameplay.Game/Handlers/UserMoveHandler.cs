using Edelstein.Common.Gameplay.Game.Objects.User;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class UserMoveHandler : AbstractFieldHandler
{
    private readonly IPipeline<FieldOnPacketUserMove> _pipeline;
    
    public override short Operation => (short)PacketRecvOperations.UserMove;

    public UserMoveHandler(IPipeline<FieldOnPacketUserMove> pipeline) => _pipeline = pipeline;
    
    protected override Task Handle(IFieldUser user, IPacketReader reader)
    {
        _ = reader.ReadLong();
        _ = reader.ReadByte();
        _ = reader.ReadLong();
        _ = reader.ReadInt();
        _ = reader.ReadInt();
        _ = reader.ReadInt();

        var message = new FieldOnPacketUserMove(user, reader.Read(new FieldUserMovePath()));

        return _pipeline.Process(message);
    }
}
