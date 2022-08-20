using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;
using Edelstein.Protocol.Util.Commands;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Plugs;

public class UserChatPlug : IPipelinePlug<IUserChat>
{
    private readonly ICommandManager<IFieldUser> _commandManager;

    public UserChatPlug(ICommandManager<IFieldUser> commandManager) => _commandManager = commandManager;

    public async Task Handle(IPipelineContext ctx, IUserChat message)
    {
        if (message.Message.StartsWith("!") || message.Message.StartsWith("@")) // TODO: config?
        {
            await _commandManager.Process(message.User, message.Message[1..]);
            return;
        }

        var chatPacket1 = new PacketWriter(PacketSendOperations.UserChat);

        chatPacket1.WriteInt(message.User.Character.ID);
        chatPacket1.WriteBool(message.User.Account.GradeCode > 0 || message.User.Account.SubGradeCode > 0);
        chatPacket1.WriteString(message.Message);
        chatPacket1.WriteBool(message.isOnlyBalloon);

        await message.User.FieldSplit!.Dispatch(chatPacket1);

        if (message.isOnlyBalloon) return;

        var chatPacket2 = new PacketWriter(PacketSendOperations.UserChatNLCPQ);

        chatPacket1.WriteInt(message.User.Character.ID);
        chatPacket1.WriteBool(message.User.Account.GradeCode > 0 || message.User.Account.SubGradeCode > 0);
        chatPacket1.WriteString(message.Message);
        chatPacket1.WriteBool(message.isOnlyBalloon);
        chatPacket2.WriteString(message.User.Character.Name);

        await Task.WhenAll(message.User.Field!.Objects
            .OfType<IFieldSplitObserver>()
            .Except(message.User.FieldSplit!.Observers)
            .Select(u => u.Dispatch(chatPacket2)));
    }
}
