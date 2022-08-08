using Edelstein.Common.Gameplay.Characters;
using Edelstein.Common.Gameplay.Inventories.Modify;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Game.Conversations;
using Edelstein.Common.Gameplay.Stages.Game.Conversations.Speakers;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Messages;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Movements;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Common.Util.Spatial;
using Edelstein.Protocol.Gameplay.Accounts;
using Edelstein.Protocol.Gameplay.Characters;
using Edelstein.Protocol.Gameplay.Inventories.Modify;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Messages;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Movements;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User;

public class FieldUser : AbstractFieldLife<IUserMovePath, IUserMoveAction>, IFieldUser
{
    public FieldUser(
        IGameStageUser user,
        IAccount account,
        IAccountWorld accountWorld,
        ICharacter character
    ) : base(new UserMoveAction(0), new Point2D(0, 0))
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
    public IConversationContext? Conversation { get; private set; }

    public bool IsInstantiated { get; set; }
    public bool IsConversing => Conversation != null;

    public ICollection<IFieldSplit> Observing { get; }
    public ICollection<IFieldControllable> Controlled { get; }

    public IPacket GetSetFieldPacket()
    {
        var packet = new PacketWriter(PacketSendOperations.SetField);

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

        return packet;
    }

    public override IPacket GetEnterFieldPacket()
    {
        var packet = new PacketWriter(PacketSendOperations.UserEnterField);

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

        return packet;
    }

    public override IPacket GetLeaveFieldPacket() =>
        new PacketWriter(PacketSendOperations.UserLeaveField)
            .WriteInt(Character.ID);

    public Task OnPacket(IPacket packet) => StageUser.OnPacket(packet);
    public Task OnException(Exception exception) => StageUser.OnException(exception);
    public Task OnDisconnect() => StageUser.OnDisconnect();
    public Task Dispatch(IPacket packet) => StageUser.Dispatch(packet);
    public Task Disconnect() => StageUser.Disconnect();

    public Task Message(string message) => Message(new SystemMessage(message));

    public async Task Message(IFieldMessage message)
    {
        var packet = new PacketWriter(PacketSendOperations.Message);

        packet.WriteByte((byte)message.Type);
        packet.Write(message);

        await Dispatch(packet);
    }

    public async Task Converse(
        IConversation conversation,
        Func<IConversationContext, IConversationSpeaker>? getSpeaker1 = null,
        Func<IConversationContext, IConversationSpeaker>? getSpeaker2 = null
    )
    {
        if (IsConversing) return;

        var ctx = new ConversationContext(this);
        var speaker1 = getSpeaker1?.Invoke(ctx) ??
                       new ConversationSpeaker(ctx, flags: ConversationSpeakerFlags.NPCReplacedByUser);
        var speaker2 = getSpeaker2?.Invoke(ctx) ??
                       new ConversationSpeaker(ctx, flags: ConversationSpeakerFlags.NPCReplacedByUser);

        Conversation = ctx;

        await Task.Run(() => conversation
            .Start(ctx, speaker1, speaker2)
            .ContinueWith(async t =>
                {
                    await EndConversation();
                    await ModifyInventory(exclRequest: true); // TODO: change to modify stats
                }
            ), ctx.TokenSource.Token);
    }

    public Task EndConversation()
    {
        if (!IsConversing) return Task.CompletedTask;
        Conversation?.Dispose();
        Conversation = null;
        return Task.CompletedTask;
    }

    public async Task ModifyInventory(Action<IModifyInventoryGroupContext>? action = null, bool exclRequest = false)
    {
        var context = new ModifyInventoryGroupContext(Character.Inventories, StageUser.Context.Templates.Item);
        var packet = new PacketWriter(PacketSendOperations.InventoryOperation);

        action?.Invoke(context);

        packet.WriteBool(exclRequest);
        packet.Write(context);
        packet.WriteBool(false);

        await Dispatch(packet);

        // TODO update stats
    }

    protected override IPacket GetMovePacket(IUserMovePath ctx)
    {
        var packet = new PacketWriter(PacketSendOperations.UserMove);

        packet.WriteInt(Character.ID);
        packet.Write(ctx);

        return packet;
    }
}
