using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Constants;
using Edelstein.Common.Gameplay.Game.Combat.Damage;
using Edelstein.Common.Gameplay.Game.Conversations;
using Edelstein.Common.Gameplay.Game.Conversations.Speakers;
using Edelstein.Common.Gameplay.Game.Objects.Dragon;
using Edelstein.Common.Gameplay.Game.Objects.User.Messages;
using Edelstein.Common.Gameplay.Models.Characters;
using Edelstein.Common.Gameplay.Models.Characters.Stats;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Common.Utilities.Spatial;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Combat.Damage;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Dragon;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Modify;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats.Modify;
using Edelstein.Protocol.Gameplay.Models.Inventories.Modify;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Services.Social;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Common.Gameplay.Game.Objects.User;

public class FieldUser : AbstractFieldLife<IFieldUserMovePath, IFieldUserMoveAction>, IFieldUser, ITickable
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

        Stats = new FieldUserStats();
        Damage = new DamageCalculator(
            user.Context.Templates.Skill    
        );

        Observing = new List<IFieldSplit>();
        Controlled = new List<IFieldObjectControllable>();
        Owned = new List<IFieldObjectOwned>();
        
        UpdateStats().Wait();
    }

    public override FieldObjectType Type => FieldObjectType.User;

    public ISocket Socket => StageUser.Socket;

    public IGameStageUser StageUser { get; }

    public IAccount Account { get; }
    public IAccountWorld AccountWorld { get; }
    public ICharacter Character { get; }

    public IFieldUserStats Stats { get; }
    public IDamageCalculator Damage { get; }

    public IConversationContext? Conversation { get; private set; }

    public bool IsInstantiated { get; set; }
    public bool IsConversing => Conversation != null;
    
    public ICollection<IFieldSplit> Observing { get; }
    public ICollection<IFieldObjectControllable> Controlled { get; }
    public ICollection<IFieldObjectOwned> Owned { get; }
    
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

        packet.WriteTemporaryStatsToRemote(Character.TemporaryStats);

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
        => Message(new SystemMessage(message));
    
    public Task Message(IPacketWritable writable)
    {
        var packet = new PacketWriter(PacketSendOperations.Message);
        packet.Write(writable);
        return Dispatch(packet.Build());
    }
    
    public async Task Effect(IPacketWritable writable, bool isLocal = true, bool isRemote = true)
    {
        var localPacket = new PacketWriter(PacketSendOperations.UserEffectLocal)
            .Write(writable)
            .Build();
        var remotePacket = new PacketWriter(PacketSendOperations.UserEffectRemote)
            .WriteInt(Character.ID)
            .Write(writable)
            .Build();

        if (isLocal)
            await Dispatch(localPacket);
        if (isRemote && FieldSplit != null) 
            await FieldSplit.Dispatch(remotePacket, this);
    }
    
    public Task EffectField(IPacketWritable writable)
    {
        var packet = new PacketWriter(PacketSendOperations.FieldEffect);
        packet.Write(writable);
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
    
    public async Task Modify(Action<IFieldUserModify> action)
    {
        var modify = new FieldUserModify(this);
        
        action.Invoke(modify);

        if (modify.IsRequireUpdate)
            await UpdateStats();
        if (modify.IsRequireUpdateAvatar) 
            await UpdateAvatar();
    }

    public Task ModifyStats(Action<IModifyStatContext>? action = null, bool exclRequest = false)
        => Modify(m => m.Stats(action, exclRequest));
    public Task ModifyStats(IModifyStatContext context, bool exclRequest = false)
        => Modify(m => m.Stats(context, exclRequest));

    public Task ModifyInventory(Action<IModifyInventoryGroupContext>? action = null, bool exclRequest = false)
        => Modify(m => m.Inventory(action, exclRequest));
    public Task ModifyInventory(IModifyInventoryGroupContext context, bool exclRequest = false)
        => Modify(m => m.Inventory(context, exclRequest));

    public Task ModifySkills(Action<IModifySkillContext>? action = null, bool exclRequest = false)
        => Modify(m => m.Skills(action, exclRequest));
    public Task ModifySkills(IModifySkillContext context, bool exclRequest = false)
        => Modify(m => m.Skills(context, exclRequest));

    public Task ModifyTemporaryStats(Action<IModifyTemporaryStatContext>? action = null, bool exclRequest = false)
        => Modify(m => m.TemporaryStats(action, exclRequest));
    public Task ModifyTemporaryStats(IModifyTemporaryStatContext context, bool exclRequest = false)
        => Modify(m => m.TemporaryStats(context, exclRequest));

    protected override IPacket GetMovePacket(IFieldUserMovePath ctx)
    {
        using var packet = new PacketWriter(PacketSendOperations.UserMove);

        packet.WriteInt(Character.ID);
        packet.Write(ctx);

        return packet.Build();
    }

    private async Task UpdateStats()
    {
        await Stats.Apply(this);
        
        if (JobConstants.GetJobRace(Character.Job) == 2 &&
            JobConstants.GetJobType(Character.Job) == 2 &&
            JobConstants.GetJobLevel(Character.Job) > 0)
        {
            var dragon = Owned
                .OfType<IFieldDragon>()
                .FirstOrDefault();

            if (dragon == null || dragon.JobCode != Character.Job)
            {
                if (dragon != null)
                {
                    Owned.Remove(dragon);
                    if (IsInstantiated && Field != null)
                        await Field.Leave(dragon);
                }

                dragon = new FieldDragon(
                    this,
                    Character.Job,
                    new FieldDragonMoveAction(0),
                    Position,
                    Foothold
                );

                Owned.Add(dragon);

                if (IsInstantiated && Field != null)
                    await Field.Enter(dragon);
            }
        }
        
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
    
    public async Task OnTick(DateTime now)
    {
        await ModifyTemporaryStats(s =>
        {
            foreach (var kv in Character.TemporaryStats.Records
                         .Where(kv => kv.Value.DateExpire < now)
                         .ToImmutableList())
                s.ResetByType(kv.Key);
            
            if ((Character.TemporaryStats.EnergyChargedRecord?.IsActive() ?? false) &&
                (Character.TemporaryStats.EnergyChargedRecord?.IsExpired(now) ?? false))
                s.ResetEnergyCharged();
            if ((Character.TemporaryStats.DashSpeedRecord?.IsActive() ?? false) &&
                (Character.TemporaryStats.DashSpeedRecord?.IsExpired(now) ?? false))
                s.ResetDashSpeed();
            if ((Character.TemporaryStats.DashJumpRecord?.IsActive() ?? false) &&
                (Character.TemporaryStats.DashJumpRecord?.IsExpired(now) ?? false))
                s.ResetDashJump();
            if ((Character.TemporaryStats.PartyBoosterRecord?.IsActive() ?? false) &&
                (Character.TemporaryStats.PartyBoosterRecord?.IsExpired(now) ?? false))
                s.ResetPartyBooster();
            if ((Character.TemporaryStats.UndeadRecord?.IsActive() ?? false) &&
                (Character.TemporaryStats.UndeadRecord?.IsExpired(now) ?? false))
                s.ResetUndead();
        });
    }
}
