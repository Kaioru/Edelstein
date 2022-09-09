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

        packet.WriteBool(false); // sNotifierMessage._m_pStr

        packet.WriteInt(0);
        packet.WriteByte((byte)(!IsInstantiated ? 1 : 2));
        packet.WriteInt(0);
        packet.WriteInt((int)(Field?.Template.Bounds.Width ?? 0));
        packet.WriteInt((int)(Field?.Template.Bounds.Height ?? 0));

        packet.WriteBool(!IsInstantiated);
        packet.WriteShort(0); // nNotifierCheck, loops

        if (!IsInstantiated)
        {
            packet.WriteUInt(0);
            packet.WriteUInt(0);
            packet.WriteUInt(0);

            packet.WriteCharacterData(Character);

            packet.WriteInt(0); // LogoutEvent
        }
        else
        {
            packet.WriteByte(0);
            packet.WriteInt(Character.FieldID);
            packet.WriteByte(Character.FieldPortal);
            packet.WriteInt(Character.HP);
            packet.WriteBool(false);
        }

        packet.WriteBool(false);
        packet.WriteBool(false);
        packet.WriteDateTime(DateTime.Now);
        packet.WriteInt(0);
        packet.WriteBool(false);
        packet.WriteBool(false);
        packet.WriteBool(false);
        packet.WriteBool(false);
        packet.WriteBool(false);
        packet.WriteBool(false);
        packet.WriteInt(0);
        packet.WriteByte(0);
        packet.WriteInt(Account.ID);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);

        return packet;
    }

    public override IPacket GetEnterFieldPacket()
    {
        var packet = new PacketWriter(PacketSendOperations.UserEnterField);

        packet.WriteInt(Character.ID);

        packet.WriteByte(Character.Level);
        packet.WriteString(Character.Name);

        packet.WriteString("");

        packet.WriteString("");
        packet.WriteShort(0);
        packet.WriteByte(0);
        packet.WriteShort(0);
        packet.WriteByte(0);

        packet.WriteByte(0);
        packet.WriteInt(0);
        packet.WriteInt(10);
        packet.WriteInt(0);

        for (var i = 0; i < 17; i++)
            packet.WriteInt(0);
        packet.WriteByte(0);
        packet.WriteByte(0);
        packet.WriteByte(0);

        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);

        packet.WriteInt(0);

        packet.WriteShort(Character.Job);
        packet.WriteShort(0);
        packet.WriteInt(0);

        packet.WriteCharacterLooks(Character);

        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteShort(-1);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);

        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WritePoint2D(Position);
        packet.WriteByte(Action.Raw);
        packet.WriteShort((short)(Foothold?.ID ?? 0));
        packet.WriteByte(0);

        packet.WriteByte(0);

        packet.WriteBool(false);
        packet.WriteBool(true);

        packet.WriteByte(0);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);

        packet.WriteBool(false);
        packet.WriteBool(false);
        packet.WriteBool(false);
        packet.WriteBool(false);
        packet.WriteBool(false);
        packet.WriteBool(false);

        packet.WriteInt(0);
        packet.WriteInt(0);

        // FARM
        packet.WriteString("");
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteByte(0);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);

        for (var i = 0; i < 5; i++)
            packet.WriteByte(255); // Event Tag Title

        packet.WriteInt(0);
        packet.WriteByte(0);

        packet.WriteByte(0);
        packet.WriteByte(0);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);

        // FREEZE HOT EVENT
        packet.WriteByte(0);
        packet.WriteInt(0);

        packet.WriteInt(0);
        packet.WriteByte(0);
        packet.WriteByte(0);
        packet.WriteInt(0);

        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteString("");
        packet.WriteInt(0);

        packet.WriteBool(false);

        packet.WriteInt(0);
        packet.WriteInt(0);
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
        var packet = new PacketWriter();

        packet.WriteByte((byte)message.Type);
        packet.Write(message);

        await Dispatch(packet);
    }

    public Task<T?> Prompt<T>(Func<IConversationSpeaker, T> prompt) =>
        Prompt((s1, s2) => prompt.Invoke(s1));

    public async Task<T?> Prompt<T>(Func<IConversationSpeaker, IConversationSpeaker, T> prompt)
    {
        var result = default(T);
        var conversation = new PromptConversation((self, target) => result = prompt.Invoke(self, target));

        await Converse(conversation);

        return result;
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

        try
        {
            await Task.Run(
                () => conversation.Start(ctx, speaker1, speaker2),
                ctx.TokenSource.Token
            );
        }
        catch (Exception)
        {
            // ignored
        }
        finally
        {
            await EndConversation();
            await ModifyInventory(exclRequest: true);
        }
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
        var packet = new PacketWriter();

        action?.Invoke(context);

        packet.WriteBool(exclRequest);
        packet.Write(context);
        packet.WriteBool(false);

        await Dispatch(packet);

        // TODO update stats
    }

    protected override IPacket GetMovePacket(IUserMovePath ctx)
    {
        var packet = new PacketWriter();

        packet.WriteInt(Character.ID);
        packet.Write(ctx);

        return packet;
    }
}
