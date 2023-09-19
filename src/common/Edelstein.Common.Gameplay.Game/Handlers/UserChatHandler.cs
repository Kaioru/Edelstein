using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class UserChatHandler : AbstractPipedFieldHandler<FieldOnPacketUserChat>
{
    public UserChatHandler(IPipeline<FieldOnPacketUserChat> pipeline) : base(pipeline)
    {
    }
    
    public override short Operation => (short)PacketRecvOperations.UserChat;

    protected override FieldOnPacketUserChat? Serialize(IFieldUser user, IPacketReader reader) 
        => new(user, reader.Skip(4).ReadString(), reader.ReadBool());
}
