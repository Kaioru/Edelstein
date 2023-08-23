﻿using Edelstein.Common.Gameplay.Models.Characters;
using Edelstein.Common.Gameplay.Models.Inventories.Modify;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Common.Utilities.Spatial;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Inventories.Modify;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.User;

public class FieldUser : AbstractFieldLife<IFieldUserMovePath, IFieldUserMoveAction>, IFieldUser
{
    public FieldUser(
        IGameStageUser user,
        IAccount account,
        IAccountWorld accountWorld,
        ICharacter character
    ) : base(new FieldUserMoveAction(0), new Point2D(0, 0))
    {
        StageUser = user;
        Account = account;
        AccountWorld = accountWorld;
        Character = character;

        Observing = new List<IFieldSplit>();
        Controlled = new List<IFieldControllable>();
    }

    public override FieldObjectType Type => FieldObjectType.User;

    public ISocket Socket => StageUser.Socket;

    public IGameStageUser StageUser { get; }

    public IAccount Account { get; }
    public IAccountWorld AccountWorld { get; }
    public ICharacter Character { get; }
    
    public bool IsInstantiated { get; set; }
    
    public ICollection<IFieldSplit> Observing { get; }
    public ICollection<IFieldControllable> Controlled { get; }

    public IPacket GetSetFieldPacket()
    {
        using var packet = new PacketWriter(PacketSendOperations.SetField);

        packet.WriteShort(0); // ClientOpt

        packet.WriteInt(StageUser.Context.Options.ChannelID);
        packet.WriteInt(StageUser.Context.Options.WorldID);

        packet.WriteBool(true); // sNotifierMessage._m_pStr
        packet.WriteBool(!IsInstantiated);
        packet.WriteShort(0); // nNotifierCheck, loops

        if (!IsInstantiated)
        {
            packet.WriteUInt(0);
            packet.WriteUInt(0);
            packet.WriteUInt(0);

            packet.WriteCharacterData(Character);

            packet.WriteInt(0);
            for (var i = 0; i < 3; i++) packet.WriteInt(0);
        }
        else
        {
            packet.WriteByte(0);
            packet.WriteInt(Character.FieldID);
            packet.WriteByte(Character.FieldPortal);
            packet.WriteInt(Character.HP);
            packet.WriteBool(false);
        }

        packet.WriteDateTime(DateTime.Now);

        return packet.Build();
    }

    public override IPacket GetEnterFieldPacket()
    {
        using var packet = new PacketWriter(PacketSendOperations.UserEnterField);

        packet.WriteInt(Character.ID);

        packet.WriteByte(Character.Level);
        packet.WriteString(Character.Name);

        packet.WriteString("");
        packet.WriteShort(0);
        packet.WriteByte(0);
        packet.WriteShort(0);
        packet.WriteByte(0);

        packet.WriteLong(0); // secondary stats
        packet.WriteLong(0);
        packet.WriteByte(0);
        packet.WriteByte(0);

        packet.WriteShort(Character.Job);
        packet.WriteCharacterLooks(Character);

        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);

        packet.WritePoint2D(Position);
        packet.WriteByte(Action.Raw);
        packet.WriteShort((short)(Foothold?.ID ?? 0));
        packet.WriteByte(0);

        packet.WriteBool(false);
        packet.WriteBool(false);
        packet.WriteBool(false);

        packet.WriteBool(false);

        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);

        packet.WriteByte(0);

        packet.WriteBool(false);

        packet.WriteBool(false);
        packet.WriteBool(false);
        packet.WriteBool(false);

        packet.WriteByte(0);

        packet.WriteByte(0);
        packet.WriteInt(0);

        return packet.Build();
    }

    public override IPacket GetLeaveFieldPacket() =>
        new PacketWriter(PacketSendOperations.UserLeaveField)
            .WriteInt(Character.ID)
            .Build();

    public Task OnPacket(IPacket packet) => StageUser.OnPacket(packet);
    public Task OnException(Exception exception) => StageUser.OnException(exception);
    public Task OnDisconnect() => StageUser.OnDisconnect();
    public Task Dispatch(IPacket packet) => StageUser.Dispatch(packet);
    public Task Disconnect() => StageUser.Disconnect();
    
    public async Task ModifyInventory(Action<IModifyInventoryGroupContext>? action = null, bool exclRequest = false)
    {
        var context = new ModifyInventoryGroupContext(Character.Inventories, StageUser.Context.Templates.Item);
        using var packet = new PacketWriter(PacketSendOperations.InventoryOperation);

        action?.Invoke(context);

        packet.WriteBool(exclRequest);
        packet.Write(context);
        packet.WriteBool(false);

        await Dispatch(packet.Build());

        // TODO update stats
    }

    protected override IPacket GetMovePacket(IFieldUserMovePath ctx)
    {
        using var packet = new PacketWriter(PacketSendOperations.UserMove);

        packet.WriteInt(Character.ID);
        packet.Write(ctx);

        return packet.Build();
    }
}
