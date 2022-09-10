using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Commands;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers;

public class UserChatHandler : AbstractFieldUserHandler
{
    private readonly ICommandManager<IFieldUser> _commandManager;

    public UserChatHandler(ICommandManager<IFieldUser> commandManager) => _commandManager = commandManager;

    public override short Operation => (short)PacketRecvOperations.UserChat;

    protected override async Task Handle(IFieldUser user, IPacketReader reader)
    {
        _ = reader.ReadInt();
        var message = reader.ReadString();
        var isOnlyBalloon = reader.ReadBool();

        if (message.StartsWith("!") || message.StartsWith("@")) // TODO: config?
        {
            await _commandManager.Process(user, message[1..]);
            return;
        }

        var packet = new PacketWriter(PacketSendOperations.UserChat);

        packet.WriteInt(user.Character.ID);
        packet.WriteBool(user.Account.GradeCode > 0 || user.Account.SubGradeCode > 0);
        packet.WriteString(message);
        packet.WriteBool(isOnlyBalloon);
        packet.WriteBool(false);
        packet.WriteByte((byte)user.StageUser.Context.Options.WorldID);

        await user.FieldSplit!.Dispatch(packet);
    }
}
