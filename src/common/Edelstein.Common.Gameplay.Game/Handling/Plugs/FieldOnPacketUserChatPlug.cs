using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketUserChatPlug : IPipelinePlug<FieldOnPacketUserChat>
{
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserChat message)
    {
        using var chatPacket1 = new PacketWriter(PacketSendOperations.UserChat);

        chatPacket1.WriteInt(message.User.Character.ID);
        chatPacket1.WriteBool(message.User.Account.GradeCode > 0 || message.User.Account.SubGradeCode > 0);
        chatPacket1.WriteString(message.Message);
        chatPacket1.WriteBool(message.IsOnlyBalloon);

        await message.User.FieldSplit!.Dispatch(chatPacket1.Build());

        if (message.IsOnlyBalloon) return;

        using var chatPacket2 = new PacketWriter(PacketSendOperations.UserChatNLCPQ);

        chatPacket2.WriteInt(message.User.Character.ID);
        chatPacket2.WriteBool(message.User.Account.GradeCode > 0 || message.User.Account.SubGradeCode > 0);
        chatPacket2.WriteString(message.Message);
        chatPacket2.WriteBool(message.IsOnlyBalloon);
        chatPacket2.WriteString(message.User.Character.Name);

        await Task.WhenAll(message.User.Field!.Objects
            .OfType<IFieldSplitObserver>()
            .Except(message.User.FieldSplit!.Observers)
            .Select(u => u.Dispatch(chatPacket2.Build())));
    }
}
