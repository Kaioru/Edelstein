using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class UserChatHandler : AbstractFieldHandler
{
    private readonly IPipeline<FieldOnPacketUserChat> _pipeline;

    public UserChatHandler(IPipeline<FieldOnPacketUserChat> pipeline) => _pipeline = pipeline;
    public override short Operation => (short)PacketRecvOperations.UserChat;

    protected override Task Handle(IFieldUser user, IPacketReader reader)
    {
        _ = reader.ReadInt();

        var message = new FieldOnPacketUserChat(user, reader.ReadString(), reader.ReadBool());

        return _pipeline.Process(message);
    }
}
