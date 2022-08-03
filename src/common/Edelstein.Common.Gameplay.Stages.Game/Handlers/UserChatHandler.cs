using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Game.Messages;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Messages;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Handlers;

public class UserChatHandler : AbstractFieldHandler
{
    private readonly IPipeline<IUserChat> _pipeline;

    public UserChatHandler(IPipeline<IUserChat> pipeline) => _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.UserChat;

    protected override Task Handle(IFieldUser user, IPacketReader reader)
    {
        _ = reader.ReadInt();

        var message = new UserChat(user, reader.ReadString(), reader.ReadBool());

        return _pipeline.Process(message);
    }
}
