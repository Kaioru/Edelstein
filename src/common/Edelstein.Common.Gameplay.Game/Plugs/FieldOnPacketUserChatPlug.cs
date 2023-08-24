﻿using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserChatPlug : IPipelinePlug<FieldOnPacketUserChat>
{
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserChat message)
    {
        var chatPacket1 = new PacketWriter(PacketSendOperations.UserChat);

        chatPacket1.WriteInt(message.User.Character.ID);
        chatPacket1.WriteBool(message.User.Account.GradeCode > 0 || message.User.Account.SubGradeCode > 0);
        chatPacket1.WriteString(message.Message);
        chatPacket1.WriteBool(message.IsOnlyBalloon);

        await message.User.FieldSplit!.Dispatch(chatPacket1.Build());

        if (message.IsOnlyBalloon) return;

        var chatPacket2 = new PacketWriter(PacketSendOperations.UserChatNLCPQ);

        chatPacket1.WriteInt(message.User.Character.ID);
        chatPacket1.WriteBool(message.User.Account.GradeCode > 0 || message.User.Account.SubGradeCode > 0);
        chatPacket1.WriteString(message.Message);
        chatPacket1.WriteBool(message.IsOnlyBalloon);
        chatPacket2.WriteString(message.User.Character.Name);

        await Task.WhenAll(message.User.Field!.Objects
            .OfType<IFieldSplitObserver>()
            .Except(message.User.FieldSplit!.Observers)
            .Select(u => u.Dispatch(chatPacket2.Build())));
    }
}
