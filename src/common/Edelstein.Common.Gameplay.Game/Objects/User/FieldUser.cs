using Edelstein.Common.Gameplay.Game.Combat;
using Edelstein.Common.Gameplay.Game.Conversations;
using Edelstein.Common.Gameplay.Game.Conversations.Speakers;
using Edelstein.Common.Gameplay.Models.Characters;
using Edelstein.Common.Gameplay.Models.Characters.Modify;
using Edelstein.Common.Gameplay.Models.Characters.Skills.Modify;
using Edelstein.Common.Gameplay.Models.Inventories.Modify;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Common.Utilities.Spatial;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Characters.Modify;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Modify;
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

        Damage = new DamageCalculator(
            user.Context.Templates.Skill    
        );

        Observing = new List<IFieldSplit>();
        Controlled = new List<IFieldControllable>();
        
        UpdateStats().Wait();
    }
    
    public override FieldObjectType Type => FieldObjectType.User;

    public ISocket Socket => StageUser.Socket;

    public IGameStageUser StageUser { get; }

    public IAccount Account { get; }
    public IAccountWorld AccountWorld { get; }
    public ICharacter Character { get; }
    
    public IFieldUserStats Stats { get; private set; }
    public IDamageCalculator Damage { get; }

    public IConversationContext? Conversation { get; private set; }

    public bool IsInstantiated { get; set; }
    public bool IsConversing => Conversation != null;

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
            packet.WriteUInt(Damage.InitSeed1);
            packet.WriteUInt(Damage.InitSeed2);
            packet.WriteUInt(Damage.InitSeed3);

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

    public Task Message(string message)
    {
        // TODO more message types
        var packet = new PacketWriter(PacketSendOperations.Message);
        packet.WriteByte(0xA);
        packet.WriteString(message);
        return Dispatch(packet.Build());
    }

    public Task<T> Prompt<T>(Func<IConversationSpeaker, T> prompt, T def) =>
        Prompt((s1, s2) => prompt.Invoke(s1), def);

    public async Task<T> Prompt<T>(Func<IConversationSpeaker, IConversationSpeaker, T> prompt, T def)
    {
        var result = def;
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
            await ModifyStats(exclRequest: true);
        }
    }

    public Task EndConversation()
    {
        if (!IsConversing) return Task.CompletedTask;
        Conversation?.Dispose();
        Conversation = null;
        return Task.CompletedTask;
    }
    
    public async Task ModifyStats(Action<IModifyStatContext>? action = null, bool exclRequest = false)
    {
        var context = new ModifyStatContext(Character);

        action?.Invoke(context);
        
        await UpdateStats();
        
        if (!IsInstantiated) return;
        
        var packet = new PacketWriter(PacketSendOperations.StatChanged);

        packet.WriteBool(exclRequest);
        packet.Write(context);
        packet.WriteBool(false);
        packet.WriteBool(false);
        
        await Dispatch(packet.Build());
    }

    public async Task ModifyInventory(Action<IModifyInventoryGroupContext>? action = null, bool exclRequest = false)
    {
        var context = new ModifyInventoryGroupContext(Character.Inventories, StageUser.Context.Templates.Item);
        using var packet = new PacketWriter(PacketSendOperations.InventoryOperation);

        action?.Invoke(context);

        packet.WriteBool(exclRequest);
        packet.Write(context);
        packet.WriteBool(false);

        if (context.IsUpdated) await UpdateStats();
        
        await Dispatch(packet.Build());

        if (context.IsUpdatedAvatar) await UpdateAvatar();
    }
    
    public async Task ModifySkills(Action<IModifySkillContext>? action = null, bool exclRequest = false)
    {
        var context = new ModifySkillContext(Character);

        action?.Invoke(context);
        
        await UpdateStats();

        var packet = new PacketWriter(PacketSendOperations.ChangeSkillRecordResult);

        packet.WriteBool(exclRequest);
        packet.Write(context);
        packet.WriteBool(true);

        await Dispatch(packet.Build());
    }

    protected override IPacket GetMovePacket(IFieldUserMovePath ctx)
    {
        using var packet = new PacketWriter(PacketSendOperations.UserMove);

        packet.WriteInt(Character.ID);
        packet.Write(ctx);

        return packet.Build();
    }

    private async Task UpdateStats()
    {
        Stats = new FieldUserStats(
            this, 
            StageUser.Context.Templates.Item,
            StageUser.Context.Templates.Skill
        );
        Console.WriteLine(Stats);
        
        if (Character.HP > Stats.MaxHP) await ModifyStats(s => s.HP = Stats.MaxHP);
        if (Character.MP > Stats.MaxMP) await ModifyStats(s => s.MP = Stats.MaxMP);
    }
    
    private async Task UpdateAvatar()
    {
        var avatarPacket = new PacketWriter(PacketSendOperations.UserAvatarModified);

        avatarPacket.WriteInt(Character.ID);
        avatarPacket.WriteByte(0x1); // Flag
        avatarPacket.WriteCharacterLooks(Character);

        avatarPacket.WriteBool(false);
        avatarPacket.WriteBool(false);
        avatarPacket.WriteBool(false);
        avatarPacket.WriteInt(0);

        if (FieldSplit != null)
            await FieldSplit.Dispatch(avatarPacket.Build(), this);
    }
}
